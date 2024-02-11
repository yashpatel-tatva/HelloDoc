using HelloDoc.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Areas.Patient.Controllers
{
    public class DashboardController : Controller
    {
        public readonly HelloDocDbContext _context;
        public DashboardController(HelloDocDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Dashboard(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            return View(user);
        }
    }
}
