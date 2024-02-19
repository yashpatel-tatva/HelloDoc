using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Area("Admin")]
        public IActionResult AdminLogin()
        {
            return View();
        }
        public IActionResult AdminForgetPassword() 
        { 
            return View(); 
        }
    }
}
