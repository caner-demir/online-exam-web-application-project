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
    public class ChoiceRepository : Repository<Choice>, IChoiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ChoiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Choice choice)
        {
            var ObjFromDb = _db.Choices.FirstOrDefault(c => c.Id == choice.Id);

            if (ObjFromDb != null)
            {
                ObjFromDb.Description = choice.Description;
            }
        }
    }
}
