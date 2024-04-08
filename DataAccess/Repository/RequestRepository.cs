using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        public HelloDocDbContext _db;
        public RequestRepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }

        public int Countbystate(string state)
        {
            if (state == "New")
            {
                return CountbyStatus(1);
            }
            else if (state == "Pending")
            {
                return CountbyStatus(2);
            }
            else if (state == "Active")
            {
                return CountbyStatus(4) + CountbyStatus(5);
            }
            else if (state == "Conclude")
            {
                return CountbyStatus(6);
            }
            else if (state == "Toclose")
            {
                return CountbyStatus(3) + CountbyStatus(7) + CountbyStatus(8);
            }
            else if (state == "Unpaid")
            {
                return CountbyStatus(9);
            }
            else
            {
                return CountbyStatus(10);
            }
        }

        public int CountbyStatus(int status)
        {
            return _db.Requests.Where(r => r.Status == status).Count();
        }

        public List<Request> GetRequestsbyState(string state)
        {
            if (state == "New")
            {
                return GetRequestsbyStatus(1);
            }
            else if (state == "Pending")
            {
                return GetRequestsbyStatus(2);
            }
            else if (state == "Active")
            {
                return GetRequestsbyStatus(4).Concat(GetRequestsbyStatus(5)).ToList();
            }
            else if (state == "Conclude")
            {
                return GetRequestsbyStatus(6);
            }
            else if (state == "Toclose")
            {
                return GetRequestsbyStatus(3).Concat(GetRequestsbyStatus(7)).Concat(GetRequestsbyStatus(8)).ToList();
            }
            else if (state == "Unpaid")
            {
                return GetRequestsbyStatus(9);
            }
            else if (state == "all")
            {
                return _db.Requests.Include(r => r.User).Include(r => r.Requestclients).Include(r => r.Physician).Include(r => r.User.Region).Include(r => r.Requeststatuslogs).Include(x => x.Requestcloseds).Include(x=>x.Requesttype).ToList(); ;
            }
            else
            {
                return GetRequestsbyStatus(10);
            }
        }

        public List<Request> GetRequestsbyStatus(int status)
        {
            return _db.Requests.Include(r => r.User).Include(r => r.Requestclients).Include(r => r.Physician).Include(r => r.User.Region).Include(r => r.Requeststatuslogs).Include(x=>x.Requesttype).Include(x=>x.Requestcloseds).Where(r => r.Status == status).ToList();
        }

        public string GetstatebyStatus(int status)
        {
            if (status == 1)
                return "New";
            else if (status == 2)
                return "Pending";
            else if (status == 5 || status == 4)
                return "Active";
            else if (status == 3 || status == 7 || status == 8)
                return "Toclose";
            else if (status == 6)
                return "Conclude";
            else if (status == 9)
                return "Unpaid";
            else if (status == 10)
                return "Close";
            else
                return "Block";
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Request request)
        {
            _db.Requests.Update(request);
        }
    }
}
