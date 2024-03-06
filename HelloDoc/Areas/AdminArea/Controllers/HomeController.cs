using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdminRepository _admin;

        public HomeController(IAdminRepository admin)
        {
            _admin = admin;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Area("AdminArea")]
        public IActionResult AdminLogin()
        {
            return View();
        }


        [Area("AdminArea")]
        public IActionResult AdminForgetPassword() 
        { 
            return View(); 
        }


        [Area("AdminArea")]
        public IActionResult AdminTabsLayout()
        {
            if (_admin.GetSessionAdminId() == -1)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            return View();
        }
    }
}
