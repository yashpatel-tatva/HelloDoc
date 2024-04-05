﻿using DataAccess.Repository.IRepository;
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

        public CredentialAdminController(
            HelloDocDbContext context,
            IAspNetUserRepository aspnetUserRepository,
            IAdminRepository adminRepository,
            IJwtRepository jwtRepository
            )
        {
            _context = context;
            _aspnetuser = aspnetUserRepository;
            _admin = adminRepository;
            _jwtRepository = jwtRepository;
        }

        [Area("AdminArea")]
        //[HttpPost]
        public async Task<IActionResult> Login(Aspnetuser user)
        {
            var alluser = _aspnetuser.GetAll().Where(x=>x.Email==user.Email).Select(x=>x.Id);
            var correct = _aspnetuser.GetFirstOrDefault(x => x.Email == user.Email);
            foreach(var u in alluser)
            {
                var Roleid = _context.Aspnetuserroles.FirstOrDefault(x => x.Userid == u).Roleid;
                if(Roleid == "1")
                {
                    correct = _aspnetuser.GetFirstOrDefault(x => x.Id == u);
                }

            }
            if (correct != null)
            {
                LoggedInPersonViewModel loggedInPersonViewModel = new LoggedInPersonViewModel();
                loggedInPersonViewModel.AspnetId = correct.Id;
                loggedInPersonViewModel.UserName = correct.Username;
                var Roleid = _context.Aspnetuserroles.FirstOrDefault(x => x.Userid == correct.Id).Roleid;
                loggedInPersonViewModel.Role = _context.Aspnetroles.FirstOrDefault(x => x.Id == Roleid).Name;
                //SessionUtilsRepository.SetLoggedInPerson(HttpContext.Session, loggedInPersonViewModel);
                var option = new CookieOptions
                {
                    Expires = DateTime.Now.AddHours(2)
                };
                Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPersonViewModel), option);
                //Response.Cookies.Append("jwt", _jwtRepository.GenerateJwtToken(loggedInPersonViewModel));
            }

            if (correct != null)
            {
                var admin = _admin.GetFirstOrDefault(u => u.Aspnetuserid == correct.Id);
                if (admin != null)
                {
                    if (_aspnetuser.checkpass(user))
                    {
                        _admin.SetSession(admin);
                        TempData["Message"] = "Welcome" + admin.Firstname + " " + admin.Lastname;
                        return RedirectToAction("AdminTabsLayout", "Home");
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
                    TempData["WrongPass"] = "You're Not admin";
                    TempData["Style"] = " border-danger";
                    return RedirectToAction("AdminLogin", "Home");
                }
            }
            else
            {
                TempData["Style"] = " border-danger";
                TempData["WrongEmail"] = "Enter Correct Email";
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
