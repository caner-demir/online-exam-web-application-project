using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExam.DataAccess.Data;
using OnlineExam.Models.ViewModels;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userList = _db.ApplicationUsers.Where(u => claim.Value != u.Id).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            var courseTotal = _db.Courses.Select(c => new { c.ApplicationUserId, c.Id });
            var courseUser = _db.CourseUsers.Where(cu => cu.IsAccepted == true);
            var allData = new List<AdminUserVM>();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                var courses = courseTotal.Where(c => c.ApplicationUserId == user.Id).Count();
                var students = courseUser.Where(s => courseTotal.Where(c => c.ApplicationUserId == user.Id).Any(c => c.Id == s.CourseId)).Count();
                allData.Add(new AdminUserVM
                {
                    ApplicationUser = user,
                    CourseCount = courses,
                    StudentCount = students
                });
            }

            return Json(new { data = allData });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _db.SaveChanges();
            return Json(new { success = true, message = "Operation Successful." });
        }
    }
}
