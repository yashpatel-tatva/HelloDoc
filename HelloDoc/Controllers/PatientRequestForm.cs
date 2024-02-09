using HelloDoc.DataContext;
using HelloDoc.DataModels;
using HelloDoc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc.Controllers
{
    public class PatientRequestForm : Controller
    {
        public readonly HelloDocDbContext _context;
        public PatientRequestForm(HelloDocDbContext context) {
            _context = context;
        }

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
                };
                _context.Aspnetusers.Add(newaspnetuser);
                _context.SaveChanges();
                User newuser = new User
                {
                    Aspnetuserid = newaspnetuser.Id,
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Email = model.Email,
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
            }
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
                    Phonenumber = model.PhoneNumber,
                    Status = model.Status,
                    Createddate = System.DateTime.Now,
                    User = user,
                };
                _context.Add(request);
                _context.SaveChanges();
                Requestclient requestclient = new Requestclient
                {
                    Notes = model.Symptomps,
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
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Regionid = (int)user.Regionid,
                };
                _context.Requestclients.Add(requestclient);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TempData["Symptomps"] = model.Symptomps;
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
