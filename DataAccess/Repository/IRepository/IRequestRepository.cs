using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRequestRepository : IRepository<Request>
    {
        void Update(Request request);
        void Save();

        int Countbystate(string state);
        int CountbyStatus(int status);
        List<Request> GetRequestsbyState(string state);
        List<Request> GetRequestsbyStatus(int status);

        string GetstatebyStatus(int status);
    }
}
