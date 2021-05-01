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
    public class CourseUserRepository : Repository<CourseUser>, ICourseUserRepository
    {
        private readonly ApplicationDbContext _db;

        public CourseUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CourseUser courseUser)
        {
            _db.Update(courseUser);
        }
    }
}
