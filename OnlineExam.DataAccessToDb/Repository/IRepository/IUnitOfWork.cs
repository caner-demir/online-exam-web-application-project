using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.DataAccessToDb.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Course { get; }
        ISP_Call SP_Call { get; }
        void Save();
    }
}
