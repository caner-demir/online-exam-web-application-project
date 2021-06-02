using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.DataAccess.Repository.IRepository;
using OnlineExam.Models;
using OnlineExam.Models.ViewModels;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineExam.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = SD.Role_Teacher)]
    public class ExamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ExamController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var course = _unitOfWork.Course.GetFirstOrDefault(c => c.Id == id);
            //Check if there is a course with this id.
            if (course == null)
            {
                return NotFound();
            }
            //Check if the user is the one who has this course.
            if (course.ApplicationUserId != claim.Value)
            {
                return NotFound();
            }

            //Save course Id to session storage.
            HttpContext.Session.SetInt32(SD.Session_SelectedCourseId, id);

            return View(course);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Get exams
            var CourseId = HttpContext.Session.GetInt32(SD.Session_SelectedCourseId);
            var AllExams = _unitOfWork.Exam.GetAll(e => e.CourseId == CourseId);

            IList<TeacherExamVM> AllData = new List<TeacherExamVM>();
            foreach (var exam in AllExams)
            {
                AllData.Add(new TeacherExamVM
                {
                    Exam = exam,
                    Questions = _unitOfWork.Question.GetAll(q => q.ExamId == exam.Id).Count()
                });
            }

            return Json(new { data = AllData });
        }

        [HttpGet]
        public IActionResult GetCounter()
        {
            var courseId = HttpContext.Session.GetInt32(SD.Session_SelectedCourseId);

            //Populate dictionary with values required for the counter panel.
            IDictionary<string, int> counterValues = new Dictionary<string, int>()
            {
                { "examCounter", _unitOfWork.Exam.GetAll(cu => cu.CourseId == courseId).Count() },
                { "studentCounter", _unitOfWork.CourseUser.GetAll(cu => cu.CourseId == courseId && cu.IsAccepted == true).Count() },
                { "questionCounter", _unitOfWork.Question.GetAll(q => q.Exam.CourseId == courseId).Count() },
                { "requestCounter", _unitOfWork.CourseUser.GetAll(cu => cu.CourseId == courseId && cu.IsAccepted == false).Count() }
            };
            return Json(new { counter = counterValues });
        }

        [NoDirectAccess]
        public IActionResult Upsert(int? id, int? courseId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Check if courseId parameter is null.
            if (courseId == null)
            {
                return NotFound();
            }
            //If courseId is not null, check if there is a course with this Id in database.
            var course = _unitOfWork.Course.GetFirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                return NotFound();
            }
            //If there is a course with this Id in database, check if the user is the owner of this course.
            if (course.ApplicationUserId != claim.Value)
            {
                return NotFound();
            }

            //If user has a course with this Id, create an Exam object.
            Exam exam = new Exam();
            if (id == null)
            {
                exam.CourseId = (int)courseId;
                var dateTime = DateTime.Now;
                exam.StartDate = dateTime.Date;
                exam.StartTime = new TimeSpan(dateTime.TimeOfDay.Hours + 1, 0, 0);
                exam.EndTime = new TimeSpan(dateTime.TimeOfDay.Hours + 1, 0, 0);
                return PartialView("_UpsertModal", exam);
            }
            else
            {
                //Check if there is an exam with this Id in database and check if the course has an 
                //exam with this Id.
                exam = _unitOfWork.Exam.GetFirstOrDefault(e => e.Id == id, includeProperties: "Course");
                if (exam == null || exam.CourseId != courseId)
                {
                    return NotFound();
                }
                //Check if the user is the owner of this exam.
                if (exam.Course.ApplicationUserId != claim.Value)
                {
                    return NotFound();
                }
            }
            exam.StartTime = exam.StartDate.TimeOfDay;
            exam.EndTime = exam.StartTime.Add(exam.Duration);

            return PartialView("_UpsertModal", exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Exam exam)
        {
            //Check if exam start date & time is greater than today's date & time.
            int dateCompare = DateTime.Compare(exam.StartDate.Add(exam.StartTime), DateTime.Now);
            //Check if exam end time is greater than exam start time.
            int timeCompare = TimeSpan.Compare(exam.EndTime, exam.StartTime);
            if (ModelState.IsValid && dateCompare > 0 && timeCompare > 0)
            {
                exam.StartDate = exam.StartDate.Add(exam.StartTime);
                exam.Duration = exam.EndTime.Subtract(exam.StartTime);
                if (exam.Id == 0)
                {
                    exam.DateCreated = DateTime.Now;
                    _unitOfWork.Exam.Add(exam);
                }
                else
                {
                    _unitOfWork.Exam.Update(exam);
                }
                _unitOfWork.Save();
                return Json(new { isValid = true });
            }
            return View(exam);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Exam.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            //----------------- Code section for deleting images of the questions belong to this exam. -----------------
            string webRootPath = _hostEnvironment.WebRootPath;
            var questionImages = _unitOfWork.Question.GetAll(q => q.ExamId == objFromDb.Id).Select(q => q.ImageUrl);
            foreach (var image in questionImages)
            {
                var imagePath = Path.Combine(webRootPath, image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _unitOfWork.Exam.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}
