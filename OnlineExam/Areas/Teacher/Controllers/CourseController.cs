using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineExam.DataAccessToDb.Repository.IRepository;
using OnlineExam.Models;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
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

        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                return RedirectToAction("AccessDenied", "Account", new { area = "Identity" });
            }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Course course)
        {
            if (ModelState.IsValid)
            {
                if (course.Id == 0)
                {
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
