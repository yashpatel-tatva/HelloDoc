using HelloDoc.Areas.Patient.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
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
        [Area("Patient")]
        public void AddPatientRequestWiseFile(List<IFormFile> formFile, int requestid)
        {
            foreach (var file in formFile)
            {
                string filename = requestid.ToString() + " _ " + file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", filename);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var data3 = new Requestwisefile()
                {
                    Requestid = requestid,
                    Filename = path,
                    Createddate = DateTime.Now,
                };

                _context.Requestwisefiles.Add(data3);

            }
            _context.SaveChanges();
        }
        [Area("Patient")]

        [HttpPost]
        public async Task<IActionResult> FamilyRequest(FamilyRequestViewModel model)
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
                    Firstname = model.F_FirstName,
                    Lastname = model.F_LastName,
                    Email = model.F_Email,
                    Phonenumber = model.F_Phone,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    Relationname = model.Relation,
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
                if (model.Upload != null)
                {
                    AddPatientRequestWiseFile(model.Upload, request.Requestid);
                }
                if(HttpContext.Session.IsAvailable)
                {
                    return RedirectToAction("Dashboard" , "Dashboard");
                }
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
