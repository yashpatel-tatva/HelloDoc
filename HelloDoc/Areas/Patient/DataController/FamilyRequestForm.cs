using HelloDoc.Areas.Patient.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc.Areas.Patient.DataController
{
    public class FamilyRequestForm : Controller
    {
        public readonly HelloDocDbContext _context;
        public FamilyRequestForm(HelloDocDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> FamilyRequest(FamilyRequestViewModel model)
        {
            var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == model.Email);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (aspnetuser != null)
            {
                Request request = new Request
                {
                    Requesttypeid = model.Requesttypeid,
                    Userid = user.Userid,
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Email = model.Email,
                    Phonenumber = model.Phone,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    Relationname = model.Relation,
                    User = user,
                };
                _context.Add(request);
                _context.SaveChanges();
                Requestclient requestclient = new Requestclient
                {
                    Notes = model.Symptopmps,
                    Requestid = request.Requestid,
                    Firstname = model.F_FirstName,
                    Lastname = model.F_LastName,
                    Email = model.F_Email,
                    Phonenumber = model.F_Phone,
                    State = model.State,
                    Street = model.Street,
                    City = model.City,
                    Zipcode = model.ZipCode,
                    Address = model.Room + " , " + model.Street + " , " + model.City + " , " + model.State,
                    Intdate = model.BirthDate.Day,
                    Intyear = model.BirthDate.Year,
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Regionid = (int)user.Regionid,
                };
                _context.Add(requestclient);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //email logic 
                return RedirectToAction("PatientRequestScreen", "Home");
            }
        }

    }
}
