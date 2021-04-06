using OnlineExam.DataAccessToDb.Data;
using OnlineExam.DataAccessToDb.Repository.IRepository;
using OnlineExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.DataAccessToDb.Repository
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
            _db.Update(exam);
        }
    }
}
