using HelloDoc.Areas.Patient.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using System.Globalization;
using System.IO;

namespace HelloDoc.Areas.Patient.DataController
{
    public class DashboardController : Controller
    {

        public readonly HelloDocDbContext _context;
        public DashboardController(HelloDocDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard(PatientDashboardViewModel patientDashboardviewmodel)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int id = (int)HttpContext.Session.GetInt32("UserId");
                PatientDashboardViewModel patientDashboard = new PatientDashboardViewModel();
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
                var request = from m in _context.Requests
                              where m.Userid == id
                              select m;
                patientDashboard.User = user;
                patientDashboard.Requests = request.ToList();

                DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
                patientDashboard.birthdate = date;
                List<Requestwisefile> files;
                if (patientDashboardviewmodel.RequestsId == 0)
                {
                     files = (from m in _context.Requestwisefiles select m).ToList();
                }
                else
                {
                     files = (from m in _context.Requestwisefiles where m.Requestid==patientDashboardviewmodel.RequestsId select m).ToList();
                    patientDashboard.RequestsId = patientDashboardviewmodel.RequestsId;
                }
                patientDashboard.requestwisefiles = files;
                patientDashboard.showdocument = patientDashboardviewmodel.showdocument;
                return View(patientDashboard);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public async Task<IActionResult> Edit(PatientDashboardViewModel model)
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);

            user.Firstname = model.User.Firstname;
            user.Lastname = model.User.Lastname;
            user.Email = model.User.Email;
            user.Street = model.User.Street;
            user.City = model.User.City;
            user.State = model.User.State;
            user.Zip = model.User.Zip;
            user.Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.birthdate.Month);
            user.Intdate = model.birthdate.Day;
            user.Intyear = model.birthdate.Year;
            user.Modifiedby = model.User.Email;
            user.Modifieddate = DateTime.Now;

            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Document(PatientDashboardViewModel model)
        {
            var requestwisefile = (from m in _context.Requestwisefiles where m.Requestid == model.RequestsId select m).ToList();
            int id = (int)HttpContext.Session.GetInt32("UserId");
            PatientDashboardViewModel patientDashboard = new PatientDashboardViewModel();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            var request = from m in _context.Requests
                          where m.Userid == id
                          select m;
            patientDashboard.User = user;
            patientDashboard.Requests = request.ToList();

            DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            patientDashboard.birthdate = date;

            patientDashboard.requestwisefiles = requestwisefile;
            patientDashboard.showdocument = model.showdocument;
            return RedirectToAction("Dashboard", patientDashboard);
        }
    }
}
