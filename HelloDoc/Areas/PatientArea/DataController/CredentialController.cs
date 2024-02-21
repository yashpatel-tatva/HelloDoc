
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class CredentialController : Controller
    {
        private readonly HelloDocDbContext _context;
        public CredentialController(HelloDocDbContext context)
        {
            _context = context;
        }

        [Area("PatientArea")]
        [Route("/Credential/checkemail/{email}")]
        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var emailExists = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
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
