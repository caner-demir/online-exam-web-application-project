using OnlineExam.DataAccess.Data;
using OnlineExam.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Course = new CourseRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            Exam = new ExamRepository(_db);
            Question = new QuestionRepository(_db);
            Choice = new ChoiceRepository(_db);
            CourseUser = new CourseUserRepository(_db);
            SP_Call = new SP_Call(_db);
        }

        public ICourseRepository Course { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IExamRepository Exam { get; private set; }
        public IQuestionRepository Question { get; private set; }
        public IChoiceRepository Choice { get; private set; }
        public ICourseUserRepository CourseUser { get; private set; }
        public ISP_Call SP_Call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
