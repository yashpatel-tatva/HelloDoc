using DataAccess.Repository.IRepository;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Areas.AdminArea.DataController
{
    public class CredentialAdminController : Controller
    {
        public readonly HelloDocDbContext _context;
        private readonly IAspNetUserRepository _aspnetuser;
        private readonly IAdminRepository _admin;

        public CredentialAdminController(
            HelloDocDbContext context , 
            IAspNetUserRepository aspnetUserRepository,
            IAdminRepository adminRepository
            )
        {
            _context = context;
            _aspnetuser = aspnetUserRepository;
            _admin = adminRepository;
        }

        [Area("AdminArea")]
        [HttpPost]
        public async Task<IActionResult> Login(Aspnetuser user)
        {
            var correct =  _aspnetuser.GetFirstOrDefault(x => x.Email == user.Email);
            if (correct != null)
            {
                var admin = _admin.GetFirstOrDefault(u => u.Aspnetuserid == correct.Id);
                if (_aspnetuser.checkemail(user))
                {
                    if (_aspnetuser.checkemail(user))
                    {

                        _admin.SetSession(admin);
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    TempData["WrongPass"] = "Enter Correct Password";
                    TempData["Style"] = " border-danger";
                    return RedirectToAction("AdminLogin", "Home");
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
            return RedirectToAction("AdminLogin", "Home");
        }
    }
}
