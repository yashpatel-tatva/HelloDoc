using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Collections;

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
        private readonly IDocumentsRepository _documents;


        public DashboardController(IAdminRepository admin, IRequestRepository request, IPhysicianRepository physicianRepository, IPaginationRepository paginationRepository, IAllRequestDataRepository allrequestdata, HelloDocDbContext db, IRequestStatusLogRepository requeststatuslog, IDocumentsRepository documents)
        {
            _admin = admin;
            _request = request;
            _physician = physicianRepository;
            _paginator = paginationRepository;
            _allrequestdata = allrequestdata;
            _db = db;
            _requeststatuslog = requeststatuslog;
            _documents = documents;
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
            var request = _request.GetFirstOrDefault(x => x.Requestid == id);
            request.Accepteddate = DateTime.Now;
            _request.Update(request);
            _request.Save();
            return RedirectToAction("Dashboard");
        }

        [Area("ProviderArea")]
        [HttpPost]
        public IActionResult DeclineCase(int id, string note)
        {
            var request = _request.GetFirstOrDefault(x => x.Requestid == id);
            request.Declinedby = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId()).Aspnetuserid;
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

        [Area("ProviderArea")]
        [HttpGet]
        public IActionResult ConcludeCare(int id)
        {
            RequestViewUploadsViewModel result = _allrequestdata.GetDocumentByRequestId(id);
            return View(result);
        }
        [Area("ProviderArea")]
        public IActionResult Download(int id)
        {
            var path = _db.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == id).Filename;
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = _documents.Download(id);
            return File(bytes, contentType, Path.GetFileName(path));
        }

        [Area("ProviderArea")]
        [HttpPost]
        public IActionResult SaveProviderNote(int requestid, string note)
        {
            _allrequestdata.SaveProviderNote(requestid, note);
            return RedirectToAction("ConcludeCare", "Dashboard", new { id = requestid });
        }
        
        [Area("ProviderArea")]
        [HttpPost]
        public IActionResult ConcludeCase(int requestid)
        {
            var request = _request.GetById(requestid);
            request.Status = 9;
            Requeststatuslog requeststatuslog = new Requeststatuslog();
            requeststatuslog.Status = 9;
            requeststatuslog.Requestid = requestid;
            requeststatuslog.Physicianid = _physician.GetSessionPhysicianId();
            requeststatuslog.Createddate = DateTime.Now;
            requeststatuslog.Notes = "case caoncluded by provider";
            _request.Update(request);
            _db.Requeststatuslogs.Add(requeststatuslog);
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        
        
        [Area("ProviderArea")]
        [HttpPost]
        public IActionResult TransferCase(int id)
        {
            return PartialView("_TransferCasePopUp" , new { requestid = id });
        }
        
        [Area("ProviderArea")]
        [HttpPost]
        public void TransfertoAdmin(int requestid , string note)
        {
            var request = _request.GetById(requestid);
            request.Status = 1;
            request.Physicianid = null;
            request.Accepteddate = null;
            request.Modifieddate = DateTime.Now;
            _request.Update(request) ;
            _request.Save();
            Requeststatuslog requeststatuslog = new Requeststatuslog() ;
            requeststatuslog.Requestid = request.Requestid;
            requeststatuslog.Status = 1;
            requeststatuslog.Physicianid = _physician.GetSessionPhysicianId() ;
            requeststatuslog.Notes = note ;
            requeststatuslog.Createddate = DateTime.Now;
            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            requeststatuslog.Transtoadmin = fortrue;
            _requeststatuslog.Add(requeststatuslog);
            _requeststatuslog.Save();

        }
    }
}
