using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExam.DataAccessToDb.Repository.IRepository;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExam.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = SD.Role_Teacher)]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            //Get course Id
            var CourseId = HttpContext.Session.GetInt32(SD.Session_SelectedCourseId);

            var students = _unitOfWork.CourseUser
                                        .GetAll(cu => (cu.IsAccepted == true) && (cu.CourseId == CourseId),
                                        includeProperties: "User,Course")
                                        .Select(cu => new { cu.User.Name, cu.User.UserName, cu.DateCreated }).ToList();

            return Json(new { data = students });
        }

        [HttpGet]
        public IActionResult GetRequests()
        {
            //Get course Id
            var CourseId = HttpContext.Session.GetInt32(SD.Session_SelectedCourseId);

            var enrollmentRequests = _unitOfWork.CourseUser
                                        .GetAll(cu => (cu.IsAccepted == false) && (cu.CourseId == CourseId),
                                        includeProperties: "User,Course")
                                        .Select(cu => new { cu.User.Name, cu.User.UserName, cu.DateCreated }).ToList();

            return Json(new { data = enrollmentRequests });
        }

        [HttpPost]
        public IActionResult Accept([FromBody] string UserName)
        {
            //Get course Id
            var CourseId = HttpContext.Session.GetInt32(SD.Session_SelectedCourseId);

            //Get user Id
            var User = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.UserName == UserName);

            var userFromDb = _unitOfWork.CourseUser.GetFirstOrDefault(cu => (cu.CourseId == CourseId) && (cu.UserId == User.Id));
            if (userFromDb == null)
            {
                return Json(new { success = false, message = "Error while enrolling the user in the course." });
            }

            userFromDb.IsAccepted = true;
            userFromDb.DateCreated = DateTime.Now;
            _unitOfWork.CourseUser.Update(userFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "The user successfully enrolled in the course." });
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]string UserName)
        {
            //Get course Id
            var CourseId = HttpContext.Session.GetInt32(SD.Session_SelectedCourseId);

            //Get user Id
            var User = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.UserName == UserName);

            var userFromDb = _unitOfWork.CourseUser.GetFirstOrDefault(cu => (cu.CourseId == CourseId) && (cu.UserId == User.Id));
            if (userFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.CourseUser.Remove(userFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}
