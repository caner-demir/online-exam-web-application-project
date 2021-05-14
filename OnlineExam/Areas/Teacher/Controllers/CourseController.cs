using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineExam.DataAccessToDb.Repository.IRepository;
using OnlineExam.Models;
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

            return Json(new { data = AllCourses });
        }

        public IActionResult Upsert(int? id)
        {
            Course course = new Course();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                course.ApplicationUserId = claim.Value;

                return View(course);
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

            return View(course);
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
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Course.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Course.Remove(objFromDb);

            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}
