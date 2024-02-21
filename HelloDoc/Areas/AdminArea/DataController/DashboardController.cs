using DataAccess.Repository.IRepository;
using DataModels.AdminSideViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.DataController
{
    public class DashboardController : Controller
    {
        private readonly HelloDocDbContext _db;
        public readonly IAdminRepository _admin;
        public readonly IRequestRepository _requests;

        public DashboardController(
            HelloDocDbContext db,
            IAdminRepository adminRepository,
            IRequestRepository requestRepository
            )
        {
            _db = db;
            _admin = adminRepository;
            _requests = requestRepository;
        }

        [Area("AdminArea")]
        public IActionResult Dashboard()
        {
            AdminDashboardViewModel model = new AdminDashboardViewModel();
            model.admin = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId());
            model.requests = _requests.GetAll().ToList();
            return View(model);
        }

        [Area("AdminArea")]
        public IActionResult Status_New() { 
            return View();
        }
       [Area("AdminArea")]
        public IActionResult Status_Pending() { 
            return View();
        }
       [Area("AdminArea")]
        public IActionResult Status_Active() { 
            return View();
        }
       [Area("AdminArea")]
        public IActionResult Status_Conclude() { 
            return View();
        }
       [Area("AdminArea")]
        public IActionResult Status_Toclose() { 
            return View();
        }
       [Area("AdminArea")]
        public IActionResult Status_Unpaid() { 
            return View();
        }
       
    }
}
