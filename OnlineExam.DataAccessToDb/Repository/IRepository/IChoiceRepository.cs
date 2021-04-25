using OnlineExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.DataAccessToDb.Repository.IRepository
{
    public interface IChoiceRepository : IRepository<Choice>
    {
        void Update(Choice choice);
    }
}
