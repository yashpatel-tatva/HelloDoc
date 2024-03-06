using HelloDoc;
using HelloDoc.Models;
using HelloDoc;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using HelloDoc.Areas.PatientArea.ViewModels;

namespace HelloDoc.Areas.PatientArea.Controllers
{
    public class RequestFormsController : Controller
    {
        private readonly HelloDocDbContext _context;
        public RequestFormsController(HelloDocDbContext context)
        {
            _context = context;
        }
        [Area("PatientArea")]
        public IActionResult PatientRequest()
        {
            //@TempData["Display"] = "d-none";
            return View();
        }
        [Area("PatientArea")]
        public IActionResult FamilyRequest()
        {
            return View();
        }
        [Area("PatientArea")]
        public IActionResult ConciergeRequest()
        {
            return View();
        }
        [Area("PatientArea")]
        public IActionResult BusinessPartnerRequest()
        {
            return View();
        }

        [Area("PatientArea")]
        [Route("/Patient/RequestForms/checkemail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _context.Aspnetusers.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
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
                    Createddate = DateTime.Now,
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



        [Area("PatientArea")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
