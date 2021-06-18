using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineExam.DataAccess.Repository.IRepository;
using OnlineExam.Models;
using OnlineExam.Models.ViewModels;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace OnlineExam.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = SD.Role_Teacher + "," + SD.Role_Admin)]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CourseController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var AllCourses = _unitOfWork.Course.GetAll(u => u.ApplicationUserId == claim.Value);
            HttpContext.Session.SetString(SD.Session_MyCourses, JsonConvert.SerializeObject(AllCourses));

            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var AllCourses = _unitOfWork.Course.GetAll(u => u.ApplicationUserId == claim.Value);

            IList<TeacherCourseVM> AllData = new List<TeacherCourseVM>();
            foreach (var course in AllCourses)
            {
                AllData.Add(new TeacherCourseVM 
                { 
                    Course = course, 
                    Students = _unitOfWork.CourseUser.GetAll(cu => cu.IsAccepted == true && cu.CourseId == course.Id).Count(),
                    Exams = _unitOfWork.Exam.GetAll(e => e.CourseId == course.Id).Count()
                });
            }

            return Json(new { data = AllData });
        }

        public IActionResult GetCounter()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Get ids of the courses the user has.
            var courseIds = _unitOfWork.Course.GetAll(u => u.ApplicationUserId == claim.Value).Select(cu => cu.Id);
            
            //Get ids of the exams the user has.
            var examIds = _unitOfWork.Exam.GetAll(cu => courseIds.Any(ci => ci == cu.CourseId));

            //Populate dictionary with values required for the counter panel.
            IDictionary<string, int> counterValues = new Dictionary<string, int>()
            {
                { "studentCounter", _unitOfWork.CourseUser.GetAll().Where(cu => courseIds.Any(ci => ci == cu.CourseId) && cu.IsAccepted == true).Count() },
                { "courseCounter", courseIds.Count() },
                { "examCounter", examIds.Count() },
                { "questionCounter", _unitOfWork.Question.GetAll().Where(q => examIds.Any(ei => ei.Id == q.ExamId)).Count() }
            };
            return Json(new { counter = counterValues });

        }

        public IActionResult Upsert(int? id)
        {
            Course course = new Course();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                course.ApplicationUserId = claim.Value;

                //return View(course);
                return PartialView("_UpsertModal", course);
            }

            course = _unitOfWork.Course.Get(id.GetValueOrDefault());
            if (course == null)
            {
                return NotFound();
            }

            if (course.ApplicationUserId != claim.Value)
            {
                return NotFound();
            }

            return PartialView("_UpsertModal", course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Course course)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\courses");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (course.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, course.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    course.ImageUrl = @"\images\courses\" + fileName + extension;
                }
                else
                {
                    if (course.Id != 0)
                    {
                        Course objFromDb = _unitOfWork.Course.Get(course.Id);
                        objFromDb.ImageUrl = course.ImageUrl;
                    }
                }

                if (course.Id == 0)
                {
                    course.DateCreated = DateTime.Now;
                    _unitOfWork.Course.Add(course);
                }
                else
                {
                    _unitOfWork.Course.Update(course);
                }
                _unitOfWork.Save();
                return Json(new { isValid = true });
            }
            return View(course);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var objFromDb = _unitOfWork.Course.Get(id);
            if (objFromDb == null || (objFromDb.ApplicationUserId != claim.Value && !User.IsInRole(SD.Role_Admin)))
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            //----------------- Code section for deleting images of the questions belong to this exam. -----------------
            //Get exams belong to this course.
            var examIds = _unitOfWork.Exam.GetAll(e => e.CourseId == objFromDb.Id).Select(e => e.Id);

            //Get questions belong to this course.
            var questionImages = _unitOfWork.Question.GetAll(q => examIds.Any(e => e == q.ExamId)).Select(q => q.ImageUrl);

            //Delete images of the questions belong to this course.
            foreach (var image in questionImages)
            {
                imagePath = Path.Combine(webRootPath, image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _unitOfWork.Course.Remove(objFromDb);

            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}
