
using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc.Areas.PatientArea.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class CredentialController : Controller
    {
        private readonly HelloDocDbContext _context;
        private readonly ISendEmailRepository _sendemail;

        public CredentialController(HelloDocDbContext context , ISendEmailRepository sendEmailRepository)
        {
            _context = context;
            _sendemail = sendEmailRepository;
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
            Aspnetuser aspnetuser = new Aspnetuser();
            User users = new User();
            Aspnetuser newaspnetuser = new Aspnetuser
            {
                Id = Guid.NewGuid().ToString(),
                Username = model.FirstName + model.LastName,
                Passwordhash = model.Password,
                Email = model.Email,
                Createddate = DateTime.Now,
            };
            _context.Aspnetusers.Add(newaspnetuser);
            _context.SaveChanges();
            User newuser = new User
            {
                Aspnetuserid = newaspnetuser.Id,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Mobile = model.PhoneNumber,
                Street = model.Street,
                City = model.City,
                State = model.State,
                Zip = model.ZipCode,
                Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                Intdate = model.BirthDate.Day,
                Intyear = model.BirthDate.Year,
                Createdby = model.Email,
                Createddate = DateTime.Now,
                Regionid = 3,
            };
            _context.Users.Add(newuser);
            _context.SaveChanges();

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
