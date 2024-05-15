
using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.CommonViewModel;
using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class CredentialController : Controller
    {
        private readonly HelloDocDbContext _context;
        private readonly ISendEmailRepository _sendemail;
        private readonly IPatientFormsRepository _patientFormsRepository;
        private readonly IAspNetUserRepository _aspnetuser;
        private readonly IJwtRepository _jwtRepository;

        public CredentialController(HelloDocDbContext context, ISendEmailRepository sendEmailRepository, IPatientFormsRepository patientFormsRepository , IAspNetUserRepository asp, IJwtRepository jwtRepository)
        {
            _context = context;
            _sendemail = sendEmailRepository;
            _patientFormsRepository = patientFormsRepository;
            _aspnetuser = asp;
            _jwtRepository = jwtRepository;
        }

        [Area("PatientArea")]
        [HttpGet]
        public void SendEmailforcreateaccount(string email)
        {
            var emaillog = _context.Emaillogs.Where(x => x.Emailid == email).OrderBy(x => x.Sentdate).LastOrDefault();
            if (emaillog != null)
            {
                if (emaillog.Sentdate < DateTime.Now.AddMinutes(-2))
                {
                    var link = "https://localhost:7249/PatientArea/Credential/CreateAccount?email=" + email;
                    _sendemail.Sendemail(email, "Create New Account", link);
                }

            }
            else
            {
                var link = "https://localhost:7249/PatientArea/Credential/CreateAccount?email=" + email;
                _sendemail.Sendemail(email, "Create New Account", link);

            }
        }

        [Area("PatientArea")]
        [HttpPost]
        public IActionResult CreateAccount(PatientRequestViewModel model)
        {
            var result = _patientFormsRepository.AddNewUserAndAspUser(model);
            if (!result)
            {
                return BadRequest("Account Alerady exist");
            }

            return RedirectToAction("PatientLogin", "Home");
        }

        [Area("PatientArea")]
        public IActionResult CreateAccount(string email)
        {
            PatientRequestViewModel aspnetuser = new PatientRequestViewModel();
            aspnetuser.Email = email;
            return View(aspnetuser);
        }


        [Area("PatientArea")]
        [HttpGet]
        public bool CheckEmail(string email)
        {
            var emailExists = _context.Users.FirstOrDefault(x => x.Email == email);
            if (emailExists == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> Login(Aspnetuser user)
        {
            var correct = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == user.Email);
            var userdata = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userdata == null && correct != null)
            {
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

                    LoggedInPersonViewModel loggedInPersonViewModel = new LoggedInPersonViewModel();
                    loggedInPersonViewModel.AspnetId = userdata.Aspnetuserid;
                    loggedInPersonViewModel.UserName = _aspnetuser.GetFirstOrDefault(x => x.Id == userdata.Aspnetuserid).Username;
                    var Roleid = _context.Aspnetuserroles.FirstOrDefault(x => x.Userid == userdata.Aspnetuserid).Roleid;
                    loggedInPersonViewModel.Role = _context.Aspnetroles.FirstOrDefault(x => x.Id == Roleid).Name;
                    var option = new CookieOptions
                    {
                        Expires = DateTime.Now.AddHours(2)
                    };
                    Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPersonViewModel), option);

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
