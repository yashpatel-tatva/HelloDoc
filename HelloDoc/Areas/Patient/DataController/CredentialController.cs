using DataAccess.Repository.IRepository;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Areas.Patient.DataController
{
    public class CredentialController : Controller
    {
        private readonly IAspNetUserRepository _context;
        public CredentialController(IAspNetUserRepository context)
        {
            _context = context;
        }

        [Route("/Credential/checkemail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _context.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }

        [HttpPost]
        public IActionResult Login(Aspnetuser user)
        {
            var result = CheckEmail(user.Email).ToString();
            result.ToString();
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
