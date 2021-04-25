using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineExam.DataAccessToDb.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Course { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IExamRepository Exam { get; }
        IQuestionRepository Question { get; }
        IChoiceRepository Choice { get; }
        ISP_Call SP_Call { get; }
        void Save();
    }
}
