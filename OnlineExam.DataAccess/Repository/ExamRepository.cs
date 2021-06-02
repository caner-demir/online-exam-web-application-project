using OnlineExam.DataAccess.Data;
using OnlineExam.DataAccess.Repository.IRepository;
using OnlineExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.DataAccess.Repository
{
    public class ExamRepository : Repository<Exam>, IExamRepository
    {
        private readonly ApplicationDbContext _db;

        public ExamRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Exam exam)
        {
            var ObjFromDb = _db.Exams.FirstOrDefault(e => e.Id == exam.Id);

            if (ObjFromDb != null)
            {
                ObjFromDb.Name = exam.Name;
                ObjFromDb.StartDate = exam.StartDate;
                ObjFromDb.Duration = exam.Duration;
                ObjFromDb.Details = exam.Details;
            }
        }
    }
}
