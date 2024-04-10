using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.CommonViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.DataController
{
    public class CredentialAdminController : Controller
    {
        public readonly HelloDocDbContext _context;
        private readonly IAspNetUserRepository _aspnetuser;
        private readonly IAdminRepository _admin;
        private readonly IJwtRepository _jwtRepository;
        private readonly IPhysicianRepository _physician;
        private readonly IUserRepository _user;
        public CredentialAdminController(
            HelloDocDbContext context,
            IAspNetUserRepository aspnetUserRepository,
            IAdminRepository adminRepository,
            IJwtRepository jwtRepository,
            IPhysicianRepository physician,
            IUserRepository userRepository
            )
        {
            _context = context;
            _aspnetuser = aspnetUserRepository;
            _admin = adminRepository;
            _jwtRepository = jwtRepository;
            _physician = physician;
            _user = userRepository;
        }

        [Area("AdminArea")]
        //[HttpPost]
        public async Task<IActionResult> Login(Aspnetuser user)
        {
            var alluser = _aspnetuser.GetAll().Where(x => x.Email == user.Email && x.Passwordhash == user.Passwordhash).Select(x => x.Id);
            Admin admin = null;
            Physician physician = null;
            User patient =null;
            Aspnetuser correct = null;
            foreach (var u in alluser)
            {
                var Roleid = _context.Aspnetuserroles.FirstOrDefault(x => x.Userid == u).Roleid;
                if (Roleid == "1")
                {
                    correct = _aspnetuser.GetFirstOrDefault(x => x.Id == u);
                    admin = _admin.GetFirstOrDefault(u => u.Aspnetuserid == correct.Id);
                }
                if (Roleid == "2")
                {
                    correct = _aspnetuser.GetFirstOrDefault(c => c.Id == u);
                    physician = _physician.GetFirstOrDefault(u => u.Aspnetuserid == correct.Id);
                }
                if (Roleid == "3")
                {
                    correct = _aspnetuser.GetFirstOrDefault(x => x.Id == u);
                    patient = _user.GetFirstOrDefault(u => u.Aspnetuserid == correct.Id);
                }
            }
            if (correct != null)
            {
                if (admin != null && physician != null)
                {
                    return RedirectToAction("SelectRole", "Home" , new { email =  user.Email , password =  user.Passwordhash });
                }
                else if (admin != null && physician == null)
                {
                    if (_aspnetuser.checkpass(user))
                    {
                        _admin.SetSession(admin);
                        TempData["Message"] = "Welcome" + admin.Firstname + " " + admin.Lastname;
                        LoggedInPersonViewModel loggedInPersonViewModel = new LoggedInPersonViewModel();
                        loggedInPersonViewModel.AspnetId = admin.Aspnetuserid;
                        loggedInPersonViewModel.UserName = _aspnetuser.GetFirstOrDefault(x => x.Id == admin.Aspnetuserid).Username;
                        var Roleid = _context.Aspnetuserroles.FirstOrDefault(x => x.Userid == admin.Aspnetuserid).Roleid;
                        loggedInPersonViewModel.Role = _context.Aspnetroles.FirstOrDefault(x => x.Id == Roleid).Name;
                        var option = new CookieOptions
                        {
                            Expires = DateTime.Now.AddHours(2)
                        };
                        Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPersonViewModel), option);
                        return RedirectToAction("AdminTabsLayout", "Home");
                    }
                    else
                    {
                        TempData["WrongPass"] = "Enter Correct Password";
                        TempData["Style"] = " border-danger";
                        return RedirectToAction("AdminLogin", "Home");
                    }

                }
                else if (admin == null && physician != null)
                {
                    if (_aspnetuser.checkpass(user))
                    {
                        _physician.SetSession(physician);
                        TempData["Message"] = "Welcome" + physician.Firstname + " " + physician.Lastname;
                        LoggedInPersonViewModel loggedInPersonViewModel = new LoggedInPersonViewModel();
                        loggedInPersonViewModel.AspnetId = physician.Aspnetuserid;
                        loggedInPersonViewModel.UserName = _aspnetuser.GetFirstOrDefault(x => x.Id == physician.Aspnetuserid).Username;
                        var Roleid = _context.Aspnetuserroles.FirstOrDefault(x => x.Userid == physician.Aspnetuserid).Roleid;
                        loggedInPersonViewModel.Role = _context.Aspnetroles.FirstOrDefault(x => x.Id == Roleid).Name;
                        var option = new CookieOptions
                        {
                            Expires = DateTime.Now.AddHours(2)
                        };
                        Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPersonViewModel), option);
                        return RedirectToAction("PhysicianTabsLayout", "Home", new { area = "ProviderArea" });
                    }
                    else
                    {
                        TempData["WrongPass"] = "Enter Correct Password";
                        TempData["Style"] = " border-danger";
                        return RedirectToAction("AdminLogin", "Home");
                    }
                }
                else
                {
                    TempData["WrongPass"] = "You're Not admin nor Provider";
                    TempData["Style"] = " border-danger";
                    return RedirectToAction("AdminLogin", "Home");
                }
            }
            else
            {
                TempData["Style"] = " border-danger";
                TempData["WrongEmail"] = "Enter Correct Credential";
                return RedirectToAction("AdminLogin", "Home");
            }
        }
        [Area("AdminArea")]
        public async Task<IActionResult> LogOut()
        {
            _admin.RemoveSession();
            Response.Cookies.Delete("jwt");
            return RedirectToAction("AdminLogin", "Home");
        }
    }
}
