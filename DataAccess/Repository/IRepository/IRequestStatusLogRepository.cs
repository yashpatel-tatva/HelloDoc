using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRequestStatusLogRepository : IRepository<RequestStatusLogRepository>
    {
        void Add(Requeststatuslog requeststatuslog);

        void Update(Requeststatuslog requeststatuslog);

        void Save();
    }
}
