using HelloDoc.Areas.PatientArea.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Globalization;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class BuisnessPartnerRequestForm : Controller
    {
        public readonly HelloDocDbContext _context;
        public BuisnessPartnerRequestForm(HelloDocDbContext context)
        {
            _context = context;
        }
        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> BuisneesPartnerRequest(BuisnessPartnerRequestViewModel model)
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
                    Firstname = model.B_FirstName,
                    Lastname = model.B_LastName,
                    Email = model.B_Email,
                    Phonenumber = model.B_Phone,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    Casenumber = model.CaseNumber,
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
                Business buisness = new Business
                {
                    Name = model.B_FirstName + model.B_LastName,
                    Address1 = model.Room + " , " + model.Street + " , " + model.City + " , " + model.State,
                    City = model.City,
                    Zipcode = model.ZipCode,
                    Createddate = DateTime.Now,
                    Phonenumber = model.B_Phone,
                    Regionid = 3,
                    Createdby = aspnetuser.Id,
                };
                _context.Add(buisness);
                _context.SaveChanges();
                var requestdata = await _context.Requests.FirstOrDefaultAsync(m => m.Email == model.Email);
                var buisnessdata = await _context.Businesses.FirstOrDefaultAsync(m => m.Name == model.B_FirstName + model.B_LastName);
                Requestbusiness requestbusiness = new Requestbusiness
                {
                    Requestid = requestdata.Requestid,
                    Businessid = buisness.Businessid,

                };
                _context.Add(requestbusiness);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("PatientRequestScreen", "Home");
            }
        }
    }
}
