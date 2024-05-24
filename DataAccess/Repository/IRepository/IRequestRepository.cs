using DataModels.AdminSideViewModels;
using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IRequestRepository : IRepository<Request>
    {
        void Update(Request request);
        void Save();

        Request GetById(int id);

        int Countbystate(string state);
        int CountbyStatus(int status);
        List<Request> GetRequestsbyState(string state);
        List<Request> GetRequestsbyStatus(int status);

        List<SearchRecodsViewModel> GetFilterdData(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile, int currentpage, int pagesize, bool order);
        string GetstatebyStatus(int status);

        int SearchRecordsCount(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile);
        byte[] GetFilterdDatatoexcle(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile, bool order);
        void DeleteThisRequest(int requestid);
        int Countbystateforprovider(string state, int providerid);
    }
}
