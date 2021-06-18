using Microsoft.AspNetCore.Authorization;
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
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

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
        [HttpGet]
        public IActionResult Index(int? page, string searchTerm = "")
        {
            int pageNumber = (page ?? 1) - 1;
            //Number of courses to display per page
            int itemsPerPage = 8;
            //Fetch courses by the page number
            var courseList = _unitOfWork.Course.GetAll(includeProperties: "ApplicationUser")
                                .Where(e => e.Name.Contains(searchTerm) || e.ApplicationUser.Name.Contains(searchTerm))
                                .OrderByDescending(c => c.DateCreated)
                                .Skip(itemsPerPage * pageNumber).Take(itemsPerPage)
                                .ToList();
            var totalItem = _unitOfWork.Course.GetAll()
                                .Where(e => e.Name.Contains(searchTerm) || e.ApplicationUser.Name.Contains(searchTerm)).Count();
            var totalUsers = _unitOfWork.CourseUser.GetAll(cu => cu.IsAccepted == true).Select(cu => cu.CourseId );

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IList<HomeCourseVM> viewData = new List<HomeCourseVM>();
            IPagedList<HomeCourseVM> pageOrders = null;
            ViewData["searchTerm"] = searchTerm;
            //Check if the user has logged in.
            if (claim != null)
            {
                //Fetch the courses the user has to display them in dropdown.
                var coursesUserHave = _unitOfWork.Course.GetAll(u => u.ApplicationUserId == claim.Value);
                HttpContext.Session.SetString(SD.Session_MyCourses, JsonConvert.SerializeObject(coursesUserHave, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
                );
                //Fetch the courses the user take to display them in dropdown.
                var coursesUserTakes = _unitOfWork.CourseUser
                                        .GetAll(cu => (cu.UserId == claim.Value) && (cu.IsAccepted == true), includeProperties: "Course")
                                        .Select(cu => new { cu.Course.Name, cu.Course.Id })
                                        .ToList();
                HttpContext.Session.SetString(SD.Session_CoursesTaken, JsonConvert.SerializeObject(coursesUserTakes));

                //For finding the courses the user has not applied or enrolled yet, first fetch the other courses.
                var coursesUserApplied = _unitOfWork.CourseUser.GetAll(cu => cu.UserId == claim.Value)
                                        .Select(cu => cu.CourseId)
                                        .ToList();
                var coursesAvailable = _unitOfWork.Course.GetAll(includeProperties: "ApplicationUser")
                                            .Where(cl => !coursesUserApplied.Any(ce => cl.Id == ce))
                                            .Where(e => e.Name.Contains(searchTerm) || e.ApplicationUser.Name.Contains(searchTerm))
                                            .OrderByDescending(c => c.DateCreated)
                                            .Skip(itemsPerPage * pageNumber).Take(itemsPerPage)
                                            .ToList();
                totalItem = _unitOfWork.Course.GetAll()
                                .Where(cl => !coursesUserApplied.Any(ce => cl.Id == ce))
                                .Where(e => e.Name.Contains(searchTerm) || e.ApplicationUser.Name.Contains(searchTerm))
                                .Count();
                foreach (var course in coursesAvailable)
                {
                    viewData.Add(new HomeCourseVM
                    {
                        Course = course,
                        Students = totalUsers.Where(cu => cu == course.Id).Count()
                    });
                }
                pageOrders = new StaticPagedList<HomeCourseVM>(viewData, pageNumber + 1, itemsPerPage, totalItem);
                //If the request is an AJAX request, return partial view.
                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    Thread.Sleep(800);
                    return PartialView("_CoursesPartial", pageOrders);
                }
                return View(pageOrders);
            }

            foreach (var course in courseList)
            {
                viewData.Add(new HomeCourseVM
                {
                    Course = course,
                    Students = totalUsers.Where(cu => cu == course.Id).Count()
                });
            }

            pageOrders = new StaticPagedList<HomeCourseVM>(viewData, pageNumber + 1, itemsPerPage, totalItem);
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                Thread.Sleep(800);
                return PartialView("_CoursesPartial", pageOrders);
            }
            return View(pageOrders);
        }

        [HttpGet]
        public IActionResult GetCounter()
        {

            //Populate the dictionary with values required for the counter panel.
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
