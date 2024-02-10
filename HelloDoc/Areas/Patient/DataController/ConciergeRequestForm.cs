using HelloDoc.Areas.Patient.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;

namespace HelloDoc.Areas.Patient.DataController
{
    public class ConciergeRequestForm : Controller
    {
        string myIP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
        public readonly HelloDocDbContext _context;

        public ConciergeRequestForm(HelloDocDbContext context) {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Conciergerequest(ConciergeRequestViewModel model)
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
                    User = user,
                };
                _context.Add(request);
                _context.SaveChanges();
                Requestclient requestclient = new Requestclient
                {
                    Notes = model.Symptopmps,
                    Requestid = request.Requestid,
                    Firstname = model.C_FirstName,
                    Lastname = model.C_LastName,
                    Email = model.C_Email,
                    Phonenumber = model.C_Phone,
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
