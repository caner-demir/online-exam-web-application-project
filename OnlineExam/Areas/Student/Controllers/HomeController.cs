using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlineExam.DataAccessToDb.Repository.IRepository;
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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll(includeProperties: "ApplicationUser");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var userCourses = _unitOfWork.Course.GetAll(u => u.ApplicationUserId == claim.Value);
                HttpContext.Session.SetString(SD.Session_MyCourses, JsonConvert.SerializeObject(userCourses));

                var coursesTaken = _unitOfWork.CourseUser
                                        .GetAll(cu => (cu.UserId == claim.Value) && (cu.IsAccepted == true),
                                        includeProperties: "Course")
                                        .Select(cu => new { cu.Course.Name, cu.Course.Id })
                                        .ToList();
                HttpContext.Session.SetString(SD.Session_CoursesTaken, JsonConvert.SerializeObject(coursesTaken));

                var coursesEnrolled = _unitOfWork.CourseUser.GetAll(cu => cu.UserId == claim.Value)
                                        .Select(cu => cu.CourseId)
                                        .ToList();

                var coursesAvailable = courseList
                                            .Where(cl => !coursesEnrolled.Any(ce => cl.Id == ce))
                                            .ToList();

                return View(coursesAvailable);
            }

            return View(courseList);
        }

        //[Authorize(Roles = SD.Role_Student + "," + SD.Role_Teacher)]
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
                IsAccepted = false
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
