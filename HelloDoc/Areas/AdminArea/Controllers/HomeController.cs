using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdminRepository _admin;
        private readonly ISendEmailRepository _sendEmail;
        private readonly IAspNetUserRepository _userRepository;
        private readonly HelloDocDbContext _db;

        public HomeController(IAdminRepository admin, ISendEmailRepository sendEmailRepository, IAspNetUserRepository aspNetUserRepository, HelloDocDbContext helloDocDbContext)
        {
            _admin = admin;
            _sendEmail = sendEmailRepository;
            _userRepository = aspNetUserRepository;
            _db = helloDocDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        //[Area("AdminArea")]
        //public IActionResult AdminLogin()
        //{
        //    return View();
        //}
        [Area("AdminArea")]
        public IActionResult AdminLogin(string email)
        {
            Aspnetuser aspnetuser = new Aspnetuser();
            aspnetuser.Email = email;
            return View(aspnetuser);
        }


        [Area("AdminArea")]
        public IActionResult AdminForgetPassword()
        {
            return View();
        }
        [Area("AdminArea")]
        public IActionResult ExpirePopUp()
        {
            return View();
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult AdminResetPassword(Aspnetuser aspnetuser)
        {
            string password = Path.GetRandomFileName();
            password = password.Replace(".", "");
            password = password.Substring(0, 8);
            var asp = _userRepository.GetFirstOrDefault(x => x.Email == aspnetuser.Email);
            if (asp == null)
            {
                TempData["Message"] = "Your Email Does not exist";
                return RedirectToAction("AdminForgetPassword");
            }
            var role = _db.Aspnetuserroles.FirstOrDefault(x => x.Userid == asp.Id).Roleid;
            if (role != "1")
            {
                TempData["Message"] = "You are not Admin";
                return RedirectToAction("AdminForgetPassword");
            }
            asp.Passwordhash = password;
            _userRepository.Update(asp);
            _userRepository.Save();
            _sendEmail.Sendemail(aspnetuser.Email, "Your OTP", "You can Login With this Password = " + password + "\n" + "You can Reset password from your profile");
            TempData["Message"] = "OTP has been sent. You can Login through it." + "\n" + "You can Reset password from your profile";
            return RedirectToAction("AdminLogin", new { email = aspnetuser.Email });
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        public IActionResult AdminTabsLayout()
        {
            var aspnetid = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().ReadJwtToken(Request.Cookies["jwt"]).Claims.FirstOrDefault(x => x.Type == "AspNetId").Value;

            if (aspnetid == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }

            _admin.SetSession(_admin.GetFirstOrDefault(x => x.Aspnetuserid == aspnetid));

            return View();
        }


    }
}
