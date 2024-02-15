using HelloDoc.DataContext;
using HelloDoc.DataModels;
using HelloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelloDoc.Areas.Patient.Controllers
{
    public class HomeController : Controller
    {
        private readonly HelloDocDbContext _context;

        public HomeController(HelloDocDbContext context)
        {
            _context = context;
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
        public IActionResult PatientForgetPassword(Aspnetuser user)
        {
            return View(user);
        }
        public IActionResult PatientResetPassword(Aspnetuser user)
        {
            return View(user);
        }
        [HttpPost]
        public IActionResult ResetPassword(Aspnetuser aspnetuser)
        {
            var aspuser = _context.Aspnetusers.FirstOrDefault(x => x.Email == aspnetuser.Email);
            aspuser.Passwordhash = aspnetuser.Passwordhash;
            _context.Aspnetusers.Update(aspuser);
            _context.SaveChanges();
            return RedirectToAction("PatientLogin");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}