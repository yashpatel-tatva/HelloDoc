using DataAccess.Repository.IRepository;
using HelloDoc;

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

        public List<Requeststatuslog> GetStatusbyId(int RequestId)
        {
            var reqstatus = _db.Requeststatuslogs.ToList().Where(x => x.Requestid == RequestId);
            return reqstatus.ToList();
        }
    public List<Requeststatuslog> GetForPhysician(int RequestId)
        {
            var requeststatuslog = _db.Requeststatuslogs.Where(x=>x.Physicianid==RequestId).ToList();
            return requeststatuslog;
        }
        public void Update(Requeststatuslog requeststatuslog)
        {
            _db.Requeststatuslogs.Update(requeststatuslog);
        }


    }
}
