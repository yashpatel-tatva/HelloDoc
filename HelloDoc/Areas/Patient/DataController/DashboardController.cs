using HelloDoc.Areas.Patient.ViewModels;
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
        
        public async Task<IActionResult> Dashboard(int id)
        {
            PatientDashboard patientDashboard = new PatientDashboard();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            var request = from m in _context.Requests
                          where m.Userid == id
                          select m;
            patientDashboard.User = user;
            patientDashboard.Requests = request.ToList();
            return View(patientDashboard);
        }
    }
}
