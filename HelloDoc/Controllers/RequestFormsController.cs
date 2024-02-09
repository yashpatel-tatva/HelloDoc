using HelloDoc.DataContext;
using HelloDoc.Models;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using HelloDoc.ViewModels;
using System.Globalization;

namespace HelloDoc.Controllers
{
    public class RequestFormsController : Controller
    {
        private readonly HelloDocDbContext _context;
        public RequestFormsController(HelloDocDbContext context)
        {
            _context = context;
        }
        public IActionResult PatientRequest()
        {
            return View();
        }
        public IActionResult FamilyRequest()
        {
            return View();
        }
        public IActionResult ConciergeRequest()
        {
            return View();
        }
        public IActionResult BuisnessPartnerRequest()
        {
            return View();
        }



        //[HttpPost]
        //public async Task<IActionResult> Self(Requestclient requestclient)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == requestclient.Email);
        //    if (user == null)
        //    {
        //        TempData["Password Fields"] = "<div class=\"row d-flex\"> <div class=\"py-2 col-xl-6 col-md-6 col-sm-12 \"> <div class=\"form-group d-flex align-items-center rounded\"> <input type=\"password\" class=\"form-control\" id=\"showpass\" placeholder=\"\" required=\"required\" asp-for=\"Email\"><span>Password</span> <label id=\"eyeiconlabel\" class=\"px-2\"><i class=\"fa-regular fa-eye-slash\" id=\"eyeicon\"></i></label> </div> </div> <div class=\"py-2 col-xl-6 col-md-6 col-sm-12\"> <div class=\"form-group d-flex align-items-center rounded\"> <input type=\"password\" class=\"form-control\" id=\"showpass1\" placeholder=\"\" required=\"required\" asp-for=\"Email\"><span>Confirm Password</span> <label id=\"eyeiconlabel1\" class=\"px-2\"><i class=\"fa-regular fa-eye-slash\" id=\"eyeicon1\"></i></label> </div> </div> </div>";

        //        return RedirectToAction("PatientRequest");
        //    }
        //    else
        //    {
        //        requestclient.Request.Userid = user.Userid;
        //        requestclient.Request.Firstname = requestclient.Firstname;
        //        requestclient.Request.Lastname = requestclient.Lastname;
        //        requestclient.Request.Email = requestclient.Email;
        //        requestclient.Request.Phonenumber = requestclient.Phonenumber;
        //        requestclient.Request.Userid = user.Userid;
        //        requestclient.Request.Isurgentemailsent.Set(0, true);
        //        requestclient.Request.Createddate = DateTime.Now;
        //        requestclient.Regionid = (int)user.Regionid;
        //        var requestdata = await _context.Requests.FirstOrDefaultAsync(m => m.Email == requestclient.Email);
        //        if (requestdata == null)
        //        {
        //            _context.Requests.Add(requestclient.Request);
        //            _context.SaveChanges();
        //            requestdata = await _context.Requests.FirstOrDefaultAsync(m => m.Email == requestclient.Email);
        //        }
        //        requestclient.Requestid = requestdata.Requestid;
        //        _context.Add(requestclient);
        //        _context.SaveChanges();
        //        return RedirectToAction("PatientRequestScreen", "Home");
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Self(PatientRequestViewModel model)
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
                    Phonenumber = model.PhoneNumber,
                    Status = model.Status,
                    Createddate = System.DateTime.Now
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
                TempData["Password Fields"] = "<div class=\"row d-flex\"> <div class=\"py-2 col-xl-6 col-md-6 col-sm-12 \"> <div class=\"form-group d-flex align-items-center rounded\"> <input type=\"password\" class=\"form-control\" id=\"showpass\" placeholder=\"\" required=\"required\" asp-for=\"Password\"><span>Password</span> <label id=\"eyeiconlabel\" class=\"px-2\"><i class=\"fa-regular fa-eye-slash\" id=\"eyeicon\"></i></label> </div> </div> <div class=\"py-2 col-xl-6 col-md-6 col-sm-12\"> <div class=\"form-group d-flex align-items-center rounded\"> <input type=\"password\" class=\"form-control\" id=\"showpass1\" placeholder=\"\" required=\"required\" ><span>Confirm Password</span> <label id=\"eyeiconlabel1\" class=\"px-2\"><i class=\"fa-regular fa-eye-slash\" id=\"eyeicon1\"></i></label> </div> </div> </div>";
                return RedirectToAction("PatientRequest");
            }
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
