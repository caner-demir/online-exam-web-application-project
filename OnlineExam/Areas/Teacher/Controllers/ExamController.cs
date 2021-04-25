using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ExamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExamController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            return Json(new { data = AllExams });
        }

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

            return PartialView("_UpsertModal", exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Exam exam)
        {
            if (ModelState.IsValid)
            {
                if (exam.Id == 0)
                {
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

            _unitOfWork.Exam.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}
