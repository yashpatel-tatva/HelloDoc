using HelloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelloDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PatientRequestScreen()
        {
            return View();
        }
        public IActionResult PatientLogin()
        {
            return View();
        }
        public IActionResult PatientForgetPassword()
        {
            return View();
        }
        public IActionResult PatientRequest()
        {
            return View("~/Views/Home/RequestForms/PatientRequest.cshtml");
        }
        public IActionResult FamilyRequest()
        {
            return View();
        }
        public IActionResult ConciergeRequest()
        {
            return View();
        }
        public IActionResult BuisnessPartnerRequest()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}