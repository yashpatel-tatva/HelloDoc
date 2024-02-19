using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Areas.Admin.DataController
{
    public class CredentialAdminController : Controller
    {
        public readonly HelloDocDbContext _context;

        public CredentialAdminController(HelloDocDbContext context)
        {
            _context = context;
        }

        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> Login(Aspnetuser user)
        {
            var correct = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == user.Email);
            if (correct != null)
            {
                var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Aspnetuserid == correct.Id);
                if (admin != null)
                {
                    if (correct.Passwordhash == user.Passwordhash)
                    {

                        int id = admin.Adminid;
                        HttpContext.Session.SetInt32("AdminId", id);
                        HttpContext.Session.SetString("UserName", admin.Firstname + " " + admin.Lastname);
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
        [Area("Admin")]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("AdminLogin", "Home");
        }
    }
}
