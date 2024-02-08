using DataAccess.Repository.IRepository;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Controllers
{
    public class CredentialController : Controller
    {
        private readonly IAspNetUserRepository _context;
        public CredentialController(IAspNetUserRepository context) { 
            _context = context;
        }
        [HttpPost]
        public IActionResult Login(Aspnetuser user)
        {
            try
            {   
                var correct = _context.GetFirstOrDefault(m => m.Email == user.Email);
                if (correct.Passwordhash == user.Passwordhash)
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
