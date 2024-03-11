
using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc.Areas.PatientArea.ViewModels;
using HelloDoc;
using HelloDoc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class CredentialController : Controller
    {
        private readonly HelloDocDbContext _context;
        private readonly ISendEmailRepository _sendemail;
        private readonly IPatientFormsRepository _patientFormsRepository;

        public CredentialController(HelloDocDbContext context , ISendEmailRepository sendEmailRepository, IPatientFormsRepository patientFormsRepository)
        {
            _context = context;
            _sendemail = sendEmailRepository;
            _patientFormsRepository = patientFormsRepository;
        }

        [Area("PatientArea")]
        [HttpGet]
        public void SendEmailforcreateaccount(string email)
        {
            var link = "https://localhost:7249/PatientArea/Credential/CreateAccount?email=" + email;
            _sendemail.Sendemail(email, "Create New Account", link);
        }

        [Area("PatientArea")]
        [HttpPost]
        public IActionResult CreateAccount(PatientRequestViewModel model)
        {
            _patientFormsRepository.AddNewUserAndAspUser(model);

            return RedirectToAction("PatientLogin","Home");
        }

        [Area("PatientArea")]
        public IActionResult CreateAccount(string email)
        {
            PatientRequestViewModel aspnetuser = new PatientRequestViewModel();
            aspnetuser.Email = email;
            return View(aspnetuser);
        }


        [Area("PatientArea")]
        [Route("/Credential/checkemail/{email}")]
        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var emailExists = await _context.Aspnetusers.FirstOrDefaultAsync(x => x.Email == email);
            return Json(new { exists = emailExists });
        }
        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> Login(Aspnetuser user)
        {
            var correct = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == user.Email);
            var userdata = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userdata == null && correct!=null) {
                TempData["Style"] = " border-danger";
                TempData["WrongEmail"] = "This is not for Admin or Provider Panel";
                return RedirectToAction("PatientLogin", "Home");
            }
            if (correct != null)
            {
                if (correct.Passwordhash == user.Passwordhash)
                {
                    int id = userdata.Userid;
                    HttpContext.Session.SetInt32("UserId", id);
                    HttpContext.Session.SetString("UserName", userdata.Firstname + " " + userdata.Lastname);
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                TempData["WrongPass"] = "Enter Correct Password";
                TempData["Style"] = " border-danger";
                return RedirectToAction("PatientLogin", "Home");
            }
            else
            {
                TempData["Style"] = " border-danger";
                TempData["WrongEmail"] = "Enter Correct Email";
                return RedirectToAction("PatientLogin", "Home");
            }
        }
        [Area("PatientArea")]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("PatientLogin", "Home");
        }
    }
}
