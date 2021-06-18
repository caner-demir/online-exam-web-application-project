using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.DataAccess.Repository.IRepository;
using OnlineExam.Models.ViewModels;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var AllCourses = _unitOfWork.Course.GetAll(includeProperties: "ApplicationUser");

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
    }
}
