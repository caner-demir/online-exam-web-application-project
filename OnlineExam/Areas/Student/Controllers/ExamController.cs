using Microsoft.AspNetCore.Mvc;
using OnlineExam.DataAccess.Repository.IRepository;
using OnlineExam.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineExam.Areas.Student.Controllers
{
    [Area("Student")]
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

            var course = _unitOfWork.Course.GetFirstOrDefault(c => c.Id == id, includeProperties: "ApplicationUser");
            //Check if there is a course with this id.
            if (course == null || claim == null)
            {
                return NotFound();
            }
            //Check if the user is enrolled in this course course.
            var courseEnrolled = _unitOfWork.CourseUser
                                    .GetFirstOrDefault(cu => (cu.CourseId == id) && (cu.UserId == claim.Value) && (cu.IsAccepted == true));
            if (courseEnrolled == null)
            {
                return NotFound();
            }

            //Save course Id to session storage.
            //HttpContext.Session.SetInt32(SD.Session_SelectedCourseId, id);

            //Return the exams belong to this course.
            var exams = _unitOfWork.Exam.GetAll(e => e.CourseId == id);

            //Fetch enrolled courses and related exams for the navigation map.
            var IdsOfCoursesEnrolled = _unitOfWork.CourseUser
                                        .GetAll(cu => (cu.UserId == claim.Value) && (cu.IsAccepted == true)).Select(cu => cu.CourseId);
            var coursesNavigation = _unitOfWork.Course.GetAll()
                                        .Where(c => IdsOfCoursesEnrolled.Any(ce => ce == c.Id));
            var examsNavigation = _unitOfWork.Exam.GetAll()
                                        .Where(e => IdsOfCoursesEnrolled.Any(ce => ce == e.CourseId));

            //Fetch ids of all students enrolled in this course.
            var countCourseUsers = _unitOfWork.CourseUser.GetAll(cu => (cu.CourseId == id) && (cu.IsAccepted == true)).Count();

            ExamsVM examsVM = new ExamsVM()
            {
                Exams = exams,
                Course = course,
                CoursesNavigation = coursesNavigation,
                ExamsNavigation = examsNavigation,
                CountCourseUsers = countCourseUsers
            };

            return View(examsVM);
        }
    }
}
