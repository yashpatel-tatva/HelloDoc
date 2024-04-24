using Entity.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.ServiceRepository.IServiceRepository;
using System.Diagnostics;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPatientServiceRepository _patient;
        public HomeController(ILogger<HomeController> logger, IPatientServiceRepository patientServiceRepository)
        {
            _logger = logger;
            _patient = patientServiceRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult OpenPatientModel(int id)
        {
            PatientViewModel model = new PatientViewModel();
            if (id == 0)
            {
                return PartialView("_PatientModel");
            }
            else
            {
                var patientdata = _patient.GetDataById(id);
                model.Id = patientdata.Id;
                model.FirstName = patientdata.FirstName;
                model.LastName = patientdata.LastName;
                model.Age = (decimal)patientdata.Age;
                model.Email = patientdata.Email;
                model.Phone = patientdata.Phone;
                model.Gender = patientdata.Gender;
                model.Dieses = patientdata.Dieses;
                model.Specialist = patientdata.Specialist;
                return PartialView("_PatientModel", model);
            }
        }
        [HttpPost]
        public void Deletethispatient(int id)
        {
            _patient.Deletethispatient(id);
        }

        [HttpPost]
        public IActionResult PatientEditOrAdd(PatientViewModel model)
        {
            var gender = Request.Form["gender"].ToList()[0];
            model.Gender = gender;
            if (model.Id == 0)
            {
                _patient.AddPatient(model);
            }
            else
            {
                _patient.EditPatient(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public bool CheckEmail(string email, int id)
        {
            var result = _patient.Checkemailexist(email, id);
            return result;
        }

        [HttpPost]
        public int PatientCountbyFilter(string search)
        {
            return _patient.PatientCountbyFilter(search);
        }
        [HttpPost]
        public IActionResult PatientData(int currentpage, int pagesize, string search)
        {
            var patients = _patient.PatientData(currentpage, pagesize, search);
            return View(patients.OrderBy(x=>x.Id).ToList()  );
        }


        [HttpPost]
        public List<string> GetDiesesData()
        {
            return _patient.GetDiesesData();
        } 
        
        [HttpPost]
        public List<string> GetDoctorData(string Dieses)
        {
            return _patient.GetDoctorData(Dieses);
        }
    }
}