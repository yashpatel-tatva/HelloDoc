using HelloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Areas.Patient.DataController
{
    public class DashboardController : Controller
    {
        public readonly HelloDocDbContext _context;
        public DashboardController(HelloDocDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> UserProfile(int id)
        {          
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            return View(user);
        }
        public async Task<IActionResult> MedicalHistory(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            TempData["user"] = user.Firstname;
            TempData["id"] = user.Userid;
            var request = from m in _context.Requests
                          where m.Userid == id
                          select m;
            return View(request.ToList());
        }
    }
}
