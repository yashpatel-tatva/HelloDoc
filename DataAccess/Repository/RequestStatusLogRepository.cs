using DataAccess.Repository.IRepository;
using HelloDoc;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RequestStatusLogRepository : Repository<RequestStatusLogRepository>, IRequestStatusLogRepository
    {
        public RequestStatusLogRepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }

        public void Add(Requeststatuslog requeststatuslog)
        {
            _db.Requeststatuslogs.Add(requeststatuslog);
        }

        public void Update(Requeststatuslog requeststatuslog)
        {
              _db.Requeststatuslogs.Update(requeststatuslog);
        }


    }
}
