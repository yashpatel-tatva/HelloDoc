using DataAccess.Repository.IRepository;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Areas.Patient.DataController
{
    public class CredentialController : Controller
    {
        private readonly HelloDocDbContext _context;
        public CredentialController(HelloDocDbContext context)
        {
            _context = context;
        }

        [Route("/Credential/checkemail/{email}")]
        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var emailExists = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return Json(new { exists = emailExists });
        }

        [HttpPost]
        public async Task<IActionResult> Login(Aspnetuser user)
        {
            try
            {
                var correct = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == user.Email);
                var userdata = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (correct.Passwordhash == user.Passwordhash)
                {
                    int id = userdata.Userid;
                    return RedirectToAction("MedicalHistory", "Dashboard", new { id = userdata.Userid});
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
