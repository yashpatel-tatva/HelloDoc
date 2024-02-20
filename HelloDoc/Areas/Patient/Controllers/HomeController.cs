using HelloDoc.DataContext;
using HelloDoc.DataModels;
using HelloDoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace HelloDoc.Areas.Patient.Controllers
{
    public class HomeController : Controller
    {
        private readonly HelloDocDbContext _context;

        public HomeController(HelloDocDbContext context)
        {
            _context = context;
        }
        [Area("Patient")]
        public IActionResult Index()
        {
            return View();
        }
        [Area("Patient")]

        public IActionResult PatientRequestScreen()
        {
            return View();
        }
        [Area("Patient")]
        public IActionResult PatientLogin()
        {
            return View();
        }
        [Area("Patient")]
        public IActionResult PatientForgetPassword(Aspnetuser user)
        {
            return View(user);
        }
        [Area("Patient")]
        public IActionResult PatientResetPassword(Aspnetuser user)
        {
            sendEmail(user.Email, "hello", "hello reset password https://localhost:44325/Home/ResetPassword/id=" + user.Email + "");

            return RedirectToAction("PatientLogin" , "Home");
        }

        public Task sendEmail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.yashpatel@outlook.com";
            var password = "Yash@7046";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }
       

        [Area("Patient")]
        [HttpPost]
        public IActionResult ResetPassword(Aspnetuser aspnetuser)
        {
            var aspuser = _context.Aspnetusers.FirstOrDefault(x => x.Email == aspnetuser.Email);
            aspuser.Passwordhash = aspnetuser.Passwordhash;
            _context.Aspnetusers.Update(aspuser);
            _context.SaveChanges();
            return RedirectToAction("PatientLogin");
        }
        [Area("Patient")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}