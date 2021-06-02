using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OnlineExam.DataAccess.Repository.IRepository;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineExam.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GetCoursesAttribute : ActionFilterAttribute
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCoursesAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var claimsIdentity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var userCourses = _unitOfWork.Course.GetAll(u => u.ApplicationUserId == claim.Value);

                filterContext.HttpContext.Session.SetString(SD.Session_MyCourses, JsonConvert.SerializeObject(userCourses, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
                                                                        );

                var coursesTaken = _unitOfWork.CourseUser
                                        .GetAll(cu => (cu.UserId == claim.Value) && (cu.IsAccepted == true),
                                        includeProperties: "Course")
                                        .Select(cu => new { cu.Course.Name, cu.Course.Id })
                                        .ToList();
                filterContext.HttpContext.Session.SetString(SD.Session_CoursesTaken, JsonConvert.SerializeObject(coursesTaken));
            }
        }
    }
}
