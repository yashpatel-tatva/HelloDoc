using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace HelloDoc.Areas.AdminArea.DataController
{
    public class DashboardController : Controller
    {
        private readonly HelloDocDbContext _db;
        public readonly IAdminRepository _admin;
        public readonly IRequestRepository _requests;
        private readonly IAllRequestDataRepository _allrequest;

        public DashboardController(
            HelloDocDbContext db,
            IAdminRepository adminRepository,
            IRequestRepository requestRepository,
            IAllRequestDataRepository allRequestDataRepository
            )
        {
            _db = db;
            _admin = adminRepository;
            _requests = requestRepository;
            _allrequest = allRequestDataRepository;
        }

        [Area("AdminArea")]
        public IActionResult Dashboard()
        {
            return View();
        }
        
        [Area("AdminArea")]
        public IActionResult Home()
        {
            AdminDashboardViewModel model = new AdminDashboardViewModel();
            if(_admin.GetSessionAdminId() == -1)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            model.admin = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId());
            model.requests = _requests.GetAll().ToList();
            return View(model);
        }


        [Area("AdminArea")]
        [HttpGet]
        public IActionResult ViewCase(int id)
        {
            var result = _allrequest.GetRequestById(id);
            return View(result);
        }
        
        
        [Area("AdminArea")]
        [HttpGet]
        public IActionResult ViewNotes(int id)
        {
            var result = _allrequest.GetNotesById(id);
            return View(result);
        }
        
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult SaveAdminNotes(RequestNotesViewModel model)
        {
            var id = model.RequestId;
            _allrequest.SaveAdminNotes(id, model);
            return RedirectToAction("AdminTabsLayout", "Home");
        }
    }
}
