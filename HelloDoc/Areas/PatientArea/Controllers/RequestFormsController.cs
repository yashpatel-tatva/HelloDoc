using HelloDoc;
using HelloDoc.Models;
using HelloDoc;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using HelloDoc.Areas.PatientArea.ViewModels;
using DataAccess.ServiceRepository.IServiceRepository;

namespace HelloDoc.Areas.PatientArea.Controllers
{
    public class RequestFormsController : Controller
    {
        private readonly HelloDocDbContext _context;
        private readonly IPatientFormsRepository _patientForm;
        public RequestFormsController(HelloDocDbContext context , IPatientFormsRepository patientFormsRepository)
        {
            _context = context;
            _patientForm = patientFormsRepository;
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
                _patientForm.AddNewUserAndAspUser(model);
            }
            var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == model.Email);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (aspnetuser != null)
            {
                _patientForm.AddRequestFromPatient(model);
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
