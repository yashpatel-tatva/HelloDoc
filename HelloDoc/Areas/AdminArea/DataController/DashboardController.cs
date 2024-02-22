using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HelloDoc.Areas.AdminArea.DataController
{
    public class DashboardController : Controller
    {
        private readonly HelloDocDbContext _db;
        public readonly IAdminRepository _admin;
        public readonly IRequestRepository _requests;
        private readonly IAllRequestDataRepository _allrequestdata;

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
            _allrequestdata = allRequestDataRepository;
        }

        [Area("AdminArea")]
        public IActionResult Dashboard()
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
        public IActionResult Status_New() {
            var model = _allrequestdata.Status(1);
            return View(model);
        }
       [Area("AdminArea")]
        public IActionResult Status_Pending() {
            var model = _allrequestdata.Status(2);
            return View(model);
        }
       [Area("AdminArea")]
        public IActionResult Status_Active() {
            var model = _allrequestdata.Status(4).Concat(_allrequestdata.Status(5)) ;
            return View(model);
        }
       [Area("AdminArea")]
        public IActionResult Status_Conclude() {
            var model = _allrequestdata.Status(6);
            return View(model);
        }
       [Area("AdminArea")]
        public IActionResult Status_Toclose() {
            var model = _allrequestdata.Status(3).Concat(_allrequestdata.Status(7)).Concat(_allrequestdata.Status(8));
            return View(model);
        }
       [Area("AdminArea")]
        public IActionResult Status_Unpaid() {
            var model = _allrequestdata.Status(9);
            return View(model);
        }
       
    }
}
