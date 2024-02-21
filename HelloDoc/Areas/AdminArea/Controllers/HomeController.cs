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
        public IActionResult AdminForgetPassword() 
        { 
            return View(); 
        }
    }
}
