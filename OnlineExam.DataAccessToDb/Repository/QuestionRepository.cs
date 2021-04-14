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
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly ApplicationDbContext _db;

        public QuestionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Question question)
        {
            _db.Update(question);
        }
    }
}
