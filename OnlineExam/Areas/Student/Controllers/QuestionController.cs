using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineExam.DataAccess.Repository.IRepository;
using OnlineExam.Models;
using OnlineExam.Models.ViewModels;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineExam.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = SD.Role_Student + "," + SD.Role_Teacher + "," + SD.Role_Admin)]
    public class QuestionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Check if there is an exam with this id.
            var exam = _unitOfWork.Exam.GetFirstOrDefault(e => e.Id == id);
            if (exam == null || claim == null)
            {
                return NotFound();
            }

            //Check if the exam is open now.
            exam.EndDate = exam.StartDate + exam.Duration;
            if (exam.StartDate > DateTime.Now || exam.EndDate < DateTime.Now)
            {
                return NotFound();
            }

            //Check if the user is enrolled in this course this exam belongs to.
            var courseEnrolled = _unitOfWork.CourseUser
                                    .GetFirstOrDefault(cu => (cu.CourseId == exam.CourseId) && (cu.UserId == claim.Value) && (cu.IsAccepted == true));
            if (courseEnrolled == null)
            {
                return NotFound();
            }

            //Save the id of the selected exam to session storage.
            HttpContext.Session.SetInt32(SD.Session_StudentSelectedExamId, id);

            var questionVM = new StudentQuestionVM()
            {
                QuestionIds = _unitOfWork.Question.GetAll(q => q.ExamId == exam.Id).Select(q => q.Id),
                CourseId = exam.CourseId,
                Duration = exam.StartDate + exam.Duration - DateTime.Now
            };

            return View(questionVM);
        }

        [HttpPost]
        public IActionResult Index([FromBody] List<Question> questions)
        {
            return Json(questions[1]);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //Get the id of the selected exam from session storage.
            var examId = HttpContext.Session.GetInt32(SD.Session_StudentSelectedExamId);

            var questions = _unitOfWork.Question.GetAll(q => q.ExamId == examId, includeProperties: "Choices").ToList();
            var questionJson = JsonConvert.SerializeObject(questions, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var questionList = JsonConvert.DeserializeObject<IEnumerable<Question>>(questionJson);

            return Json(new { data = questionList });
        }
    }
}
