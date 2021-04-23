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

            var exam = _unitOfWork.Exam.GetFirstOrDefault(e => e.Id == id, includeProperties: "Course");
            //Check if there is an exam with this id.
            if (exam == null)
            {
                return NotFound();
            }
            //Check if the user is the one who has this exam.
            if (exam.Course.ApplicationUserId != claim.Value)
            {
                return NotFound();
            }

            //Save exam Id to session storage.
            HttpContext.Session.SetInt32(SD.Session_SelectedExamId, id);

            return View(exam);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Get questions
            var ExamId = HttpContext.Session.GetInt32(SD.Session_SelectedExamId);
            var AllQuestions = _unitOfWork.Question.GetAll(q => q.ExamId == ExamId);

            return Json(new { data = AllQuestions });
        }

        [NoDirectAccess]
        public IActionResult Upsert(int? id, int? examId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Check if examId parameter is null.
            if (examId == null)
            {
                return NotFound();
            }
            //If examId is not null, check if there is an exam with this Id in database.
            var exam = _unitOfWork.Exam.GetFirstOrDefault(e => e.Id == examId, includeProperties: "Course");
            if (exam == null)
            {
                return NotFound();
            }
            //If there is an exam with this Id in database, check if the user is the owner of this exam.
            if (exam.Course.ApplicationUserId != claim.Value)
            {
                return NotFound();
            }

            //If user has an exam with this Id, create a Question object.
            Question question = new Question();
            if (id == null)
            {
                question.ExamId = (int)examId;
                return PartialView("_UpsertModal", question);
            }
            else
            {
                //Check if there is a question with this Id in database and check if the exam has a
                //question with this Id.
                question = _unitOfWork.Question.GetFirstOrDefault(e => e.Id == id, includeProperties: "Exam");
                if (question == null || question.ExamId != examId)
                {
                    return NotFound();
                }
                //Check if the user is the owner of this exam.
                if (exam.Course.ApplicationUserId != claim.Value)
                {
                    return NotFound();
                }
            }

            return PartialView("_UpsertModal", question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Question question)
        {
            if (ModelState.IsValid)
            {
                if (question.Id == 0)
                {
                    _unitOfWork.Question.Add(question);
                }
                else
                {
                    _unitOfWork.Question.Update(question);
                }
                _unitOfWork.Save();
                return Json(new { isValid = true });
            }
            return View(question);
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Question.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Question.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}
