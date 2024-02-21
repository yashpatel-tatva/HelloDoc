using HelloDoc.Areas.PatientArea.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Globalization;
using System.Net;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class ConciergeRequestForm : Controller
    {
        public readonly HelloDocDbContext _context;

        public ConciergeRequestForm(HelloDocDbContext context) {
            _context = context;
        }
        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> Conciergerequest(ConciergeRequestViewModel model)
        {
            var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == model.Email);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.Regionid == user.Regionid);
            var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
            if (aspnetuser != null)
            {
                Request request = new Request
                {
                    Requesttypeid = model.Requesttypeid,
                    Userid = user.Userid,
                    Firstname = model.C_FirstName,
                    Lastname = model.C_LastName,
                    Email = model.C_Email,
                    Phonenumber = model.C_Phone,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    Confirmationnumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                    User = user,
                };
                _context.Add(request);
                _context.SaveChanges();
                Requestclient requestclient = new Requestclient
                {
                    Notes = model.Symptoms,
                    Requestid = request.Requestid,
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Email = model.Email,
                    Phonenumber = model.Phone,
                    State = model.C_State,
                    Street = model.C_Street,
                    City = model.C_City,
                    Zipcode = model.C_ZipCode,
                    Address = model.Room + " , " + model.C_Street + " , " + model.C_City + " , " + model.C_State,
                    Intdate = model.BirthDate.Day,
                    Intyear = model.BirthDate.Year,
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Regionid = (int)user.Regionid,
                };
                _context.Add(requestclient);
                _context.SaveChanges();
                Concierge concierge = new Concierge {
                    Conciergename = model.C_FirstName + model.C_LastName,
                    Address = model.Room + " , " + model.C_Street + " , " + model.C_City + " , " + model.C_State,
                    State = model.C_State,
                    Street = model.C_Street,
                    City = model.C_City,
                    Zipcode = model.C_ZipCode,
                    Createddate = DateTime.Now,
                    Regionid = 3,
                };
                _context.Add(concierge);
                _context.SaveChanges();
                var requestdata = await _context.Requests.FirstOrDefaultAsync(m => m.Email == model.Email);
                var conciergedata = await _context.Concierges.FirstOrDefaultAsync(m => m.Conciergename == model.C_FirstName + model.C_LastName);
                Requestconcierge requestconcierge = new Requestconcierge
                {
                    Requestid = requestdata.Requestid,
                    Conciergeid = conciergedata.Conciergeid,
                    
                };
                _context.Add(requestconcierge);
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
