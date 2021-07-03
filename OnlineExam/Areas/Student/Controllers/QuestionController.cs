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

        [HttpGet]
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

            //Check if the user has submitted the answers of this exam before.
            var userAnswers = _unitOfWork.ExamResult.GetFirstOrDefault(es => (es.ApplicationUserId == claim.Value) && (es.ExamId == id));
            if (userAnswers != null)
            {
                return Json(new { status = false });
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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Get the id of the selected exam from session storage.
            var examId = HttpContext.Session.GetInt32(SD.Session_StudentSelectedExamId);
            //Get the questions of the exam from db.
            var questionsFromDb = _unitOfWork.Question.GetAll(q => q.ExamId == examId).ToList();
            //Create new examResult object.
            var score = 0;
            var examResult = new ExamResult()
            {
                ApplicationUserId = claim.Value,
                ExamId = examId ?? 0,
                Score = score
            };
            _unitOfWork.ExamResult.Add(examResult);
            _unitOfWork.Save();
            examResult = _unitOfWork.ExamResult.GetFirstOrDefault(e => e.ApplicationUserId == claim.Value && e.ExamId == examId);
            //Compare selected choices with correct choices for each question.
            foreach (var question in questions)
            {
                var questionSelected = questionsFromDb.Where(q => q.Id == question.Id).FirstOrDefault();
                if (question.CorrectChoice == questionSelected.CorrectChoice)
                {
                    score += question.Points;
                }
                var questionToDb = new QuestionResult()
                {
                    ChoiceSelected = question.CorrectChoice,
                    QuestionId = questionSelected.Id,
                    ExamResultId = examResult.Id
                };
                _unitOfWork.QuestionResult.Add(questionToDb);
            }

            examResult.Score = score;
            _unitOfWork.ExamResult.Update(examResult);
            _unitOfWork.Save();

            return Json(Url.Action("Index", "Exam", new { Area = "Student", id = HttpContext.Session.GetInt32(SD.Session_SelectedCourseId) }));
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
