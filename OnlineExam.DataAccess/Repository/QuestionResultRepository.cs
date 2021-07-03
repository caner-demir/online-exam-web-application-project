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
    public class QuestionResultRepository : Repository<QuestionResult>, IQuestionResultRepository
    {
        private readonly ApplicationDbContext _db;

        public QuestionResultRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(QuestionResult questionResult)
        {
            //_db.Update(question);
            var ObjFromDb = _db.QuestionResults.FirstOrDefault(q => q.Id == questionResult.Id);

            if (ObjFromDb != null)
            {
                ObjFromDb.ChoiceSelected = questionResult.ChoiceSelected;
            }
        }
    }
}
