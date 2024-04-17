using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.CommonViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace HelloDoc.Areas.AdminArea.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdminRepository _admin;
        private readonly ISendEmailRepository _sendEmail;
        private readonly IAspNetUserRepository _userRepository;
        private readonly HelloDocDbContext _db;
        private readonly IJwtRepository _jwtRepository;
        private readonly IPhysicianRepository _physician;

        public HomeController(IAdminRepository admin, ISendEmailRepository sendEmailRepository, IAspNetUserRepository aspNetUserRepository, HelloDocDbContext helloDocDbContext, IJwtRepository jwtRepository, IPhysicianRepository physician)
        {
            _admin = admin;
            _sendEmail = sendEmailRepository;
            _userRepository = aspNetUserRepository;
            _db = helloDocDbContext;
            _jwtRepository = jwtRepository;
            _physician = physician;
        }

        [Area("AdminArea")]
        public IActionResult Index()
        {
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            if (token != null)
            {
                jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
                var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

                if (roleClaim != null)
                {
                    var role = roleClaim.Value;
                    if (role == "Admin")
                    {
                        return RedirectToAction("AdminTabsLayout", "Home");
                    }
                    if (role == "Physician")
                    {
                        return RedirectToAction("PhysicianTabsLayout", "Home", new { area = "ProviderArea" });
                    }
                }
            }
            return RedirectToAction("AdminLogin", "Home");
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
            var jwt = HttpContext.Request.Cookies["jwt"];
            if (jwt != null)
            {
                if (_jwtRepository.ValidateToken(jwt, out JwtSecurityToken jwttoken))
                {
                    var aspnetuserid = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
                    var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
                    var aspnetuserforlog = _userRepository.GetFirstOrDefault(x => x.Id == aspnetuserid.Value);
                    if (roleClaim.Value == "Admin")
                    {
                        return RedirectToAction("Login", "CredentialAdmin", aspnetuserforlog);
                    }
                }
            }
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
            //string password = Path.GetRandomFileName();
            //password = password.Replace(".", "");
            //password = password.Substring(0, 8);
            //var asp = _userRepository.GetFirstOrDefault(x => x.Email == aspnetuser.Email);
            //if (asp == null)
            //{
            //    TempData["Message"] = "Your Email Does not exist";
            //    return RedirectToAction("AdminForgetPassword");
            //}
            //var role = _db.Aspnetuserroles.FirstOrDefault(x => x.Userid == asp.Id).Roleid;
            //if (role != "1")
            //{
            //    TempData["Message"] = "You are not Admin";
            //    return RedirectToAction("AdminForgetPassword");
            //}
            //asp.Passwordhash = password;
            //_userRepository.Update(asp);
            //_userRepository.Save();
            //_sendEmail.Sendemail(aspnetuser.Email, "Your OTP", "You can Login With this Password = " + password + "\n" + "You can Reset password from your profile");
            //TempData["Message"] = "OTP has been sent. You can Login through it." + "\n" + "You can Reset password from your profile";

            string Id = (_db.Aspnetusers.FirstOrDefault(x => x.Email == aspnetuser.Email)).Id;
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("ResetPassword", "Home", new { id = Id });
            string resetPasswordUrl = baseUrl + resetPasswordPath;
            _sendEmail.Sendemail(aspnetuser.Email, "Reset Password Link", resetPasswordUrl);
            TempData["Message"] = "Reset Link Sent" + "\n" + "You can Reset password from that page";
            return RedirectToAction("AdminLogin", new { email = aspnetuser.Email });
        }

        [Area("AdminArea")]
        public IActionResult ResetPassword(string id)
        {
            var aspuser = _db.Aspnetusers.FirstOrDefault(x => x.Id == id);
            var email = _db.Emaillogs.Where(x => x.Emailid == aspuser.Email).Where(x => x.Subjectname == "Reset Password Link").OrderBy(x => x.Sentdate).LastOrDefault();
            if (email.Sentdate < DateTime.Now.AddDays(-1))
            {
                return PartialView("_passwordError");
            }
            return View(aspuser);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ResetPasswordOfthis(Aspnetuser aspnetuser)
        {
            var aspuser = _db.Aspnetusers.FirstOrDefault(x => x.Email == aspnetuser.Email);
            aspuser.Passwordhash = aspnetuser.Passwordhash;
            _db.Aspnetusers.Update(aspuser);
            _db.SaveChanges();
            return RedirectToAction("AdminLogin");
        }

        [Area("AdminArea")]
        public IActionResult SelectRole(string email, string password)
        {
            return View(new { email = email, password = password });
        }

        [Area("AdminArea")]
        public IActionResult SelectedRole(string email, string password, string role)
        {
            var aspnetusers = _userRepository.GetAll().Where(x => x.Email == email && x.Passwordhash == password);
            Admin admin = new Admin();
            Physician physician = new Physician();
            foreach (var user in aspnetusers)
            {
                var asp = _db.Aspnetuserroles.Where(x => x.Userid == user.Id && x.Roleid == role).FirstOrDefault();
                if (asp != null)
                {
                    var aspid = asp.Userid.ToString();
                    if (role == "1")
                    {
                        admin = _admin.GetFirstOrDefault(z => z.Aspnetuserid == aspid);
                    }
                    if (role == "2")
                    {
                        physician = _physician.GetFirstOrDefault(x => x.Aspnetuserid == aspid);
                    }
                }
            }
            if (role == "1")
            {
                _admin.SetSession(admin);
                TempData["Message"] = "Welcome" + admin.Firstname + " " + admin.Lastname;
                LoggedInPersonViewModel loggedInPersonViewModel = new LoggedInPersonViewModel();
                loggedInPersonViewModel.AspnetId = admin.Aspnetuserid;
                loggedInPersonViewModel.UserName = _userRepository.GetFirstOrDefault(x => x.Id == admin.Aspnetuserid).Username;
                var Roleid = _db.Aspnetuserroles.FirstOrDefault(x => x.Userid == admin.Aspnetuserid).Roleid;
                loggedInPersonViewModel.Role = _db.Aspnetroles.FirstOrDefault(x => x.Id == Roleid).Name;
                var option = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(2)
                };
                Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPersonViewModel), option);
                return RedirectToAction("AdminTabsLayout", "Home");
            }
            if (role == "2")
            {
                _physician.SetSession(physician);
                TempData["Message"] = "Welcome" + physician.Firstname + " " + physician.Lastname;
                LoggedInPersonViewModel loggedInPersonViewModel = new LoggedInPersonViewModel();
                loggedInPersonViewModel.AspnetId = physician.Aspnetuserid;
                loggedInPersonViewModel.UserName = _userRepository.GetFirstOrDefault(x => x.Id == physician.Aspnetuserid).Username;
                var Roleid = _db.Aspnetuserroles.FirstOrDefault(x => x.Userid == physician.Aspnetuserid).Roleid;
                loggedInPersonViewModel.Role = _db.Aspnetroles.FirstOrDefault(x => x.Id == Roleid).Name;
                var option = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(2)
                };
                Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPersonViewModel), option);
                return RedirectToAction("PhysicianTabsLayout", "Home", new { area = "ProviderArea" });
            }
            return View(new { email = email, password = password });
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
