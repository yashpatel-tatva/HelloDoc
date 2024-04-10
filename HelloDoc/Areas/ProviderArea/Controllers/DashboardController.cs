using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    [AuthorizationRepository("Physician")]
    public class DashboardController : Controller
    {
        private readonly IAdminRepository _admin;
        private readonly IPhysicianRepository _physician;
        private readonly IRequestRepository _request;
        private readonly IPaginationRepository _paginator;
        private readonly IAllRequestDataRepository _allrequestdata;
        private readonly IRequestStatusLogRepository _requeststatuslog;
        private readonly HelloDocDbContext _db;

        public DashboardController(IAdminRepository admin, IRequestRepository request, IPhysicianRepository physicianRepository, IPaginationRepository paginationRepository, IAllRequestDataRepository allrequestdata, HelloDocDbContext db, IRequestStatusLogRepository requeststatuslog)
        {
            _admin = admin;
            _request = request;
            _physician = physicianRepository;
            _paginator = paginationRepository;
            _allrequestdata = allrequestdata;
            _db = db;
            _requeststatuslog = requeststatuslog;
        }

        [Area("ProviderArea")]
        public IActionResult Dashboard()
        {
            AdminDashboardViewModel model = new AdminDashboardViewModel();
            model.Physician = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId());
            model.newrequest = _request.Countbystateforprovider("New", _physician.GetSessionPhysicianId());
            model.pendingrequest = _request.Countbystateforprovider("Pending", _physician.GetSessionPhysicianId());
            model.activerequest = _request.Countbystateforprovider("Active", _physician.GetSessionPhysicianId());
            model.concluderequest = _request.Countbystateforprovider("Conclude", _physician.GetSessionPhysicianId());
            return View(model);
        }

        [Area("ProviderArea")]
        [HttpPost]
        public IActionResult StatuswiseData(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        {
            return RedirectToAction("Status_" + state, new { state, currentpage, pagesize, requesttype, search, region });
        }
        [Area("ProviderArea")]
        public IActionResult Status_New(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        {
            List<Request> model1 = _paginator.requestsofProvider(state, currentpage, pagesize, requesttype, search, region, _physician.GetSessionPhysicianId());
            List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
            return View(filtereddata);
        }
        [Area("ProviderArea")]
        public IActionResult Status_Pending(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        {
            List<Request> model1 = _paginator.requestsofProvider(state, currentpage, pagesize, requesttype, search, region, _physician.GetSessionPhysicianId());
            List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
            return View(filtereddata);
        }
        [Area("ProviderArea")]
        public IActionResult Status_Active(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        {
            List<Request> model1 = _paginator.requestsofProvider(state, currentpage, pagesize, requesttype, search, region, _physician.GetSessionPhysicianId());
            List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
            return View(filtereddata);
        }
        [Area("ProviderArea")]
        public IActionResult Status_Conclude(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        {
            List<Request> model1 = _paginator.requestsofProvider(state, currentpage, pagesize, requesttype, search, region, _physician.GetSessionPhysicianId());
            List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
            return View(filtereddata);
        }
        [Area("ProviderArea")]
        [HttpPost]
        public int CountbyFilter(string state, int requesttype, string search, int region)
        {
            List<Request> model1 = _paginator.requestsofProvider(state, 0, 0, requesttype, search, region, _physician.GetSessionPhysicianId());
            return model1.Count();
        }

        [Area("ProviderArea")]
        [HttpPost]
        public IActionResult AcceptCase(int id)
        {
            var request = _request.GetFirstOrDefault(x=>x.Requestid == id);
            request.Accepteddate = DateTime.Now;
            _request.Update(request);
            _request.Save();
            return RedirectToAction("Dashboard");
        }
        
        [Area("ProviderArea")]
        [HttpPost]
        public IActionResult DeclineCase(int id , string note)
        {
            var request = _request.GetFirstOrDefault(x=>x.Requestid == id);
            request.Declinedby = _physician.GetFirstOrDefault(x=>x.Physicianid == _physician.GetSessionPhysicianId()).Aspnetuserid;
            request.Accepteddate = null;
            request.Status = 1;
            request.Physicianid = null;
            _request.Update(request);
            _request.Save();
            var requestsstatuslog = new Requeststatuslog();
            requestsstatuslog.Requestid = id;
            requestsstatuslog.Status = 1;
            requestsstatuslog.Physicianid = _physician.GetSessionPhysicianId();
            requestsstatuslog.Notes = note;
            requestsstatuslog.Createddate = DateTime.Now;
            _requeststatuslog.Add(requestsstatuslog);
            _requeststatuslog.Save();
            return RedirectToAction("Dashboard");
        }

    }
}
