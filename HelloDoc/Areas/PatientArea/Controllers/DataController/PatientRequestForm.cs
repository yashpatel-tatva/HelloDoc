using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class PatientRequestForm : Controller
    {
        public readonly HelloDocDbContext _context;
        public PatientRequestForm(HelloDocDbContext context)
        {
            _context = context;
        }
        [Area("PatientArea")]
        public void AddPatientRequestWiseFile(List<IFormFile> formFile, int requestid)
        {
            foreach (var file in formFile)
            {
                string filename = file.FileName;
                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
                string extension = Path.GetExtension(filename);
                string filewith = filenameWithoutExtension + "_" + DateTime.Now.ToString("dd`MM`yyyy`HH`mm`ss") + extension;

                //string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HalloDoc Request Documents", requestid.ToString());
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", requestid.ToString());

                if (!Directory.Exists(directoryPath))
                {
                    // Create the directory
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, filewith);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }


                var data3 = new Requestwisefile()
                {
                    Requestid = requestid,
                    Filename = filePath,
                    Createddate = DateTime.Now,
                };

                _context.Requestwisefiles.Add(data3);

            }
            _context.SaveChanges();
        }

        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> Self(PatientRequestViewModel model)
        {


            if (model.Password != null)
            {
                Aspnetuser newaspnetuser = new Aspnetuser
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = model.FirstName + model.LastName,
                    Passwordhash = model.Password,
                    Email = model.Email,
                    Createddate = DateTime.Now,
                    Phonenumber = model.PhoneNumber,
                };
                _context.Aspnetusers.Add(newaspnetuser);
                _context.SaveChanges();
                User newuser = new User
                {
                    Aspnetuserid = newaspnetuser.Id,
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Email = model.Email,
                    Mobile = model.PhoneNumber,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    Zip = model.ZipCode,
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Intdate = model.BirthDate.Day,
                    Intyear = model.BirthDate.Year,
                    Createdby = model.Email,
                    Createddate = DateTime.Now,
                    Regionid = 3,
                };
                _context.Users.Add(newuser);
                _context.SaveChanges();
                Aspnetuserrole aspnetuserrole = new Aspnetuserrole();
                aspnetuserrole.Userid = newaspnetuser.Id;
                aspnetuserrole.Roleid = "3";
                _context.Aspnetuserroles.Add(aspnetuserrole);
                _context.SaveChanges();
            }
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
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    Phonenumber = user.Mobile,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    User = user,
                    Confirmationnumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
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
                    Phonenumber = model.PhoneNumber,
                    State = model.State,
                    Street = model.Street,
                    City = model.City,
                    Zipcode = model.ZipCode,
                    Intdate = model.BirthDate.Day,
                    Intyear = model.BirthDate.Year,
                    Address = model.Room + " , " + model.Street + " , " + model.City + " , " + model.State,
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Regionid = (int)user.Regionid,
                };
                _context.Requestclients.Add(requestclient);
                _context.SaveChanges();
                if (model.Upload != null)
                {
                    AddPatientRequestWiseFile(model.Upload, request.Requestid);
                }
                int id = user.Userid;
                HttpContext.Session.SetInt32("UserId", id);
                HttpContext.Session.SetString("UserName", user.Firstname + " " + user.Lastname);
                return RedirectToAction("Dashboard", "Dashboard");
            }
            else
            {
                //TempData["Symptoms"] = model.Symptoms;
                //ViewBag.FirstName = model.FirstName;
                //ViewBag.LastName = model.LastName;
                //ViewBag.Email = model.Email;
                //ViewBag.PhoneNumber = model.PhoneNumber;
                //ViewBag.State = model.State;
                //ViewBag.Street = model.Street;
                //ViewBag.City = model.City;
                //ViewBag.ZipCode = model.ZipCode;
                //TempData["Password Fields"] = "<div class=\"row d-flex\"> <div class=\"py-2 col-xl-6 col-md-6 col-sm-12 \"> <div class=\"form-group d-flex align-items-center rounded\"> <input type=\"password\" class=\"form-control\" id=\"showpass\" placeholder=\"\" required=\"required\" asp-for=\"Password\"><span>Password</span> <label id=\"eyeiconlabel\" class=\"px-2\"><i class=\"fa-regular fa-eye-slash\" id=\"eyeicon\"></i></label> </div> </div> <div class=\"py-2 col-xl-6 col-md-6 col-sm-12\"> <div class=\"form-group d-flex align-items-center rounded\"> <input type=\"password\" class=\"form-control\" id=\"showpass1\" placeholder=\"\" required=\"required\" ><span>Confirm Password</span> <label id=\"eyeiconlabel1\" class=\"px-2\"><i class=\"fa-regular fa-eye-slash\" id=\"eyeicon1\"></i></label> </div> </div> </div>";
                //@TempData["Display"] = "d-flex";
                return RedirectToAction("PatientRequest", model);
            }
        }
    }
}
