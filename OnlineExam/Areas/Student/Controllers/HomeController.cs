﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlineExam.DataAccess;
using OnlineExam.DataAccess.Repository.IRepository;
using OnlineExam.Models;
using OnlineExam.Models.ViewModels;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineExam.Areas.Student.Controllers
{
    [Area("Student")]
    //[ServiceFilter(typeof(Filters.GetCoursesAttribute))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        //[ServiceFilter(typeof(GetCoursesAttribute))]
        public IActionResult Index()
        {
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll(includeProperties: "ApplicationUser");
            var countUsers = _unitOfWork.CourseUser.GetAll(cu => cu.IsAccepted == true).Select(cu => cu.CourseId );

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IList<HomeCourseVM> AllData = new List<HomeCourseVM>();
            if (claim != null)
            {
                var userCourses = _unitOfWork.Course.GetAll(u => u.ApplicationUserId == claim.Value);

                HttpContext.Session.SetString(SD.Session_MyCourses, JsonConvert.SerializeObject(userCourses, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
                                                                        );

                var coursesTaken = _unitOfWork.CourseUser
                                        .GetAll(cu => (cu.UserId == claim.Value) && (cu.IsAccepted == true),
                                        includeProperties: "Course")
                                        .Select(cu => new { cu.Course.Name, cu.Course.Id })
                                        .ToList();
                HttpContext.Session.SetString(SD.Session_CoursesTaken, JsonConvert.SerializeObject(coursesTaken));

                //For finding the courses the user has not enrolled yet, fetch the courses user is taking.
                var coursesEnrolled = _unitOfWork.CourseUser.GetAll(cu => cu.UserId == claim.Value)
                                        .Select(cu => cu.CourseId)
                                        .ToList();

                var coursesAvailable = courseList
                                            .Where(cl => !coursesEnrolled.Any(ce => cl.Id == ce))
                                            .ToList();

                foreach (var course in coursesAvailable)
                {
                    AllData.Add(new HomeCourseVM
                    {
                        Course = course,
                        Students = countUsers.Where(cu => cu == course.Id).Count()
                    });
                }
                return View(AllData);
            }

            foreach (var course in courseList)
            {
                AllData.Add(new HomeCourseVM
                {
                    Course = course,
                    Students = countUsers.Where(cu => cu == course.Id).Count()
                });
            }
            return View(AllData);
        }

        [HttpGet]
        public IActionResult GetCounter()
        {

            //Populate dictionary with values required for the counter panel.
            IDictionary<string, int> counterValues = new Dictionary<string, int>()
            {
                { "userCounter", _unitOfWork.ApplicationUser.GetAll().Count() },
                { "courseCounter", _unitOfWork.Course.GetAll().Count() },
                { "examCounter", _unitOfWork.Exam.GetAll().Count() },
                { "questionCounter", _unitOfWork.Question.GetAll().Count() }
            };
            return Json(new { counter = counterValues });
        }

        [Authorize]
        [HttpPost]
        public IActionResult SendRequest([FromBody]int Id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var CourseUser = new CourseUser()
            {
                CourseId = Id,
                UserId = claim.Value,
                IsAccepted = false,
                DateCreated = DateTime.Now
            };

            _unitOfWork.CourseUser.Add(CourseUser);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Enrollment request sent." });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
