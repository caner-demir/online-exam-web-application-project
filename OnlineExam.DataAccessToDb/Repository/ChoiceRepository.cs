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
    public class ChoiceRepository : Repository<Choice>, IChoiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ChoiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Choice choice)
        {
            _db.Update(choice);
        }
    }
}
