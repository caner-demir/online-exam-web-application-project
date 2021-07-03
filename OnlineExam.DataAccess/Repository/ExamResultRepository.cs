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
    public class ExamResultRepository : Repository<ExamResult>, IExamResultRepository
    {
        private readonly ApplicationDbContext _db;

        public ExamResultRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ExamResult ExamResult)
        {
            var ObjFromDb = _db.ExamResults.FirstOrDefault(e => e.Id == ExamResult.Id);

            if (ObjFromDb != null)
            {
                ObjFromDb.Score = ExamResult.Score;
            }
        }
    }
}
