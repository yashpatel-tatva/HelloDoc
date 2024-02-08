 using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Controllers
{
    public class CredentialController : Controller
    {
        private readonly HelloDocDbContext _context;
        public CredentialController(HelloDocDbContext context) { 
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            try
            {   
                var correct = await _context.Aspnetusers
                .FirstOrDefaultAsync(m => m.Email == user.Email);
                if (correct.Passwordhash == user.Aspnetuser.Passwordhash)
                {
                    return RedirectToAction("Index", "Home");
                }
                TempData["WrongPass"] = "Enter Correct Password";
                TempData["Style"] = " border-danger";
                return RedirectToAction("PatientLogin", "Home");
            }
            catch
            {
                TempData["Style"] = " border-danger";
                TempData["WrongEmail"] = "Enter Correct Email";
                return RedirectToAction("PatientLogin", "Home");
            }
        }
    }
}
