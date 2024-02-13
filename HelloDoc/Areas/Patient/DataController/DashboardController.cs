using HelloDoc.Areas.Patient.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
            PatientDashboardViewModel patientDashboard = new PatientDashboardViewModel();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            var request = from m in _context.Requests
                          where m.Userid == id
                          select m;
            patientDashboard.User = user;
            patientDashboard.Requests = request.ToList();

            DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            patientDashboard.birthdate = date;

            List<Requestwisefile> files = (from m in _context.Requestwisefiles  select m).ToList();
            patientDashboard.requestwisefiles = files;

            return View(patientDashboard);
       }

        public async Task<IActionResult> Edit(int id , PatientDashboardViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            PatientDashboardViewModel patientDashboard = new PatientDashboardViewModel();
            patientDashboard.User = model.User;
            patientDashboard.User.Aspnetuserid = user.Aspnetuserid;
            patientDashboard.User.Createdby = user.Createdby;
            patientDashboard.User.Createddate = user.Createddate;
            patientDashboard.User.Modifieddate = DateTime.Now;
            User userdata = new User()
            {

            };

            _context.Users.Update(patientDashboard.User);
            _context.SaveChanges();
            return RedirectToAction("Dashboard" , "Dashboard" , new {id = id});

        }
    }
}
