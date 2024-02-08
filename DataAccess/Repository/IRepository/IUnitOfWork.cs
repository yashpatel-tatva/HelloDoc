using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAspNetUserRepository AspNetUser { get; }
        IUserRepository User { get; }
        IRequestRepository Request { get; }

        void Save();
    }
}
