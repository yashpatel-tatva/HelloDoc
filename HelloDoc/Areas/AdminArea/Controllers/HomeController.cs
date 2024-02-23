using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers
{
    public class HomeController : Controller
    {
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
            return View();
        }
    }
}
