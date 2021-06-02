using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineExam.DataAccessToDb.Data;
using OnlineExam.DataAccessToDb.Repository.IRepository;
using OnlineExam.Models;
using OnlineExam.Models.ViewModels;
using OnlineExam.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IWebHostEnvironment _hostEnvironment;

        public QuestionController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
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
            var AllQuestions = _unitOfWork.Question.GetAll(q => q.ExamId == ExamId, includeProperties: "Choices");

            //For preventing loop serialize and derialize the allQuestions data.
            var jsonData = JsonConvert.SerializeObject(AllQuestions, new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });

            var dataDeseriazed = JsonConvert.DeserializeObject<IEnumerable<Question>>(jsonData);

            return Json(new { data = dataDeseriazed });
        }

        [HttpGet]
        public IActionResult GetCounter(int id)
        {
            var courseId = _unitOfWork.Exam.Get(id)?.CourseId;

            //Populate dictionary with values required for the counter panel.
            IDictionary<string, int> counterValues = new Dictionary<string, int>();
            if (courseId != null)
            {
                counterValues = new Dictionary<string, int>()
                {
                    { "questionCounter", _unitOfWork.Question.GetAll(q => q.ExamId == id).Count() },
                    { "pointsCounter", _unitOfWork.Question.GetAll(q => q.ExamId == id).Select(q => q.Points).Sum() },
                    { "studentCounter", _unitOfWork.CourseUser.GetAll(cu => cu.CourseId == courseId && cu.IsAccepted == true).Count() }
                };
            }
            return Json(new { counter = counterValues });
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
            //If examId is not null, then check if there is an exam with this Id in database.
            var exam = _unitOfWork.Exam.GetFirstOrDefault(e => e.Id == examId, includeProperties: "Course");
            if (exam == null)
            {
                return NotFound();
            }
            //If there is an exam with this Id in database, then check if the user is the owner of this exam.
            if (exam.Course.ApplicationUserId != claim.Value)
            {
                return NotFound();
            }

            Question question = new Question();
            if (id == null)
            {
                question.ExamId = (int)examId;
                return PartialView("_UpsertModal", question);
            }
            else
            {
                //Check if there is a question with this Id in database AND check if the exam has a
                //question with this Id.
                question = _unitOfWork.Question.GetFirstOrDefault(q => q.Id == id);
                if (question == null || question.ExamId != examId)
                {
                    return NotFound();
                }
                //Check if the user is the owner of this exam.
                if (exam.Course.ApplicationUserId != claim.Value)
                {
                    return NotFound();
                }
                question = _unitOfWork.Question.GetFirstOrDefault(q => q.Id == id, includeProperties: "Choices");
            }

            return PartialView("_UpsertModal", question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Question question)
        {
            if (ModelState.IsValid && question.CorrectChoice != null && question.Choices.Count() <= 5)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\questions");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (question.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, question.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    question.ImageUrl = @"\images\questions\" + fileName + extension;
                }
                else
                {
                    if (question.Id != 0)
                    {
                        Question ObjFromDb = _unitOfWork.Question.Get(question.Id);
                        question.ImageUrl = ObjFromDb.ImageUrl;
                    }
                }

                if (question.Id == 0)
                {
                    question.DateCreated = DateTime.Now;

                    _unitOfWork.Question.Add(question);
                    foreach (var choice in question.Choices)
                    {
                        _unitOfWork.Choice.Add(choice);
                    }
                }
                else
                {
                    _unitOfWork.Question.Update(question);

                    var choicesFromDb = _unitOfWork.Choice.GetAll(c => c.QuestionId == question.Id).ToList();
                    var choicesRemoved = choicesFromDb.Where(c => !question.Choices.Any(q => q.Id == c.Id));
                    foreach (var choice in choicesRemoved)
                    {
                        _unitOfWork.Choice.Remove(choice);
                    }

                    foreach (var choice in question.Choices)
                    {
                        if (choice.Id != 0)
                        {
                            _unitOfWork.Choice.Update(choice);
                        }
                        if (choice.Id == 0)
                        {
                            choice.QuestionId = question.Id;
                            _unitOfWork.Choice.Add(choice);
                        }
                    }
                }
                _unitOfWork.Save();
                return Json(new { isValid = true });
            }
            else
            {
                return View(question);
            }            
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Question.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _unitOfWork.Question.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}
