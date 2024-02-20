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
        public IActionResult PatientResetPasswordEmail(Aspnetuser user)
        {
            string Id = (_context.Aspnetusers.FirstOrDefault(x => x.Email == user.Email)).Id;
            string resetPasswordUrl = GenerateResetPasswordUrl(Id);
            SendEmail(user.Email, "Reset Your Password", $"Hello, reset your password using this link: {resetPasswordUrl}");

            return RedirectToAction("PatientLogin", "Home");
        }

        [Area("Patient")]
        private string GenerateResetPasswordUrl(string userId)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("PatientResetPassword", "Home", new { id = userId });
            return baseUrl + resetPasswordPath;
        }


        [Area("Patient")]
        private Task SendEmail(string email, string subject, string message)
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

        // Handle the reset password URL in the same controller or in a separate one
        [Area("Patient")]
        public IActionResult PatientResetPassword(string id)
        {
            var aspuser = _context.Aspnetusers.FirstOrDefault(x => x.Id == id);

            return View(aspuser);
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