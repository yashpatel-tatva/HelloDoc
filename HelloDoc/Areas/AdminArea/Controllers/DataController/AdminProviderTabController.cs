using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AdminProviderTabController : Controller
    {
        private readonly IProviderMenuRepository _providerMenu;
        private readonly ISendEmailRepository _sendEmail;
        private readonly IPhysicianRepository _physician;
        private readonly IAdminRepository _admin;
        private readonly IAspNetUserRepository _userRepository;
        private readonly IRoleRepository _role;
        private readonly HelloDocDbContext _db;
        public AdminProviderTabController(IProviderMenuRepository providerMenu, ISendEmailRepository sendEmail, IPhysicianRepository physicianRepository, IAdminRepository adminRepository, IAspNetUserRepository userRepository, HelloDocDbContext helloDocDbContext, IRoleRepository role)
        {
            _providerMenu = providerMenu;
            _sendEmail = sendEmail;
            _physician = physicianRepository;
            _admin = adminRepository;
            _userRepository = userRepository;
            _db = helloDocDbContext;
            _role = role;
        }

        /// <summary>
        /// Providers
        /// </summary>
        /// <returns></returns>

        [Area("AdminArea")]
        public IActionResult Providers()
        {
            return View();
        }

        [Area("AdminArea")]
        public IActionResult Providersfilter(int region, int order)
        {
            var physicians = _providerMenu.GetAllProviderDetailToDisplay(region, order);
            return PartialView("_ProvidersDetail", physicians);
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ChangeNotification(List<int> checkedToUnchecked, List<int> uncheckedToChecked)
        {
            _providerMenu.ChangeNotification(checkedToUnchecked, uncheckedToChecked);
            TempData["Message"] = "Changes Saved";
            return RedirectToAction("AdminTabsLayout", "Home");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ContactProviderPopUp(int physicianid)
        {
            return PartialView("_ContactProvider", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public void ContactThisProvider(int physicianid, string msgtype, string msg)
        {
            if (msgtype == "typeemail" || msgtype == "typeboth")
            {
                _sendEmail.Sendemail(_physician.GetFirstOrDefault(x => x.Physicianid == physicianid).Email, "Message From" + _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Firstname + "(Admin)", msg);
            }
        }


        [Area("AdminArea")]
        //[HttpPost]
        public IActionResult EditProviderPage(int physicianid)
        {
            PhysicianAccountViewModel model = new PhysicianAccountViewModel();
            model = _providerMenu.GetPhysicianAccountById(physicianid);
            return View(model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public void ProviderResetPassword(string aspnetid, string password)
        {
            _userRepository.changepass(aspnetid, password);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderPersonal([FromBody] PhysicianAccountViewModel viewModel)
        {
            _providerMenu.EditPersonalinfo(viewModel);
            return RedirectToAction("EditProviderPage", new { physicianid = viewModel.PhysicianId });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderMailingInfo([FromBody] PhysicianAccountViewModel viewModel)
        {
            _providerMenu.EditProviderMailingInfo(viewModel);
            return RedirectToAction("EditProviderPage", new { physicianid = viewModel.PhysicianId });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderAuthenticationInfo([FromBody] PhysicianAccountViewModel viewModel)
        {
            _providerMenu.EditProviderAuthenticationInfo(viewModel);
            return RedirectToAction("EditProviderPage", new { physicianid = viewModel.PhysicianId });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderAdminNote(int physicianid, string adminnote)
        {
            _providerMenu.EditProviderAdminNote(physicianid, adminnote);
            return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderPhoto(int physicianid, string base64string)
        {
            _providerMenu.EditProviderPhoto(physicianid, base64string);
            return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderSign(int physicianid, string base64string)
        {
            _providerMenu.EditProviderSign(physicianid, base64string);
            return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult OpenSignPadPopUp(int physicianid)
        {
            return PartialView("_SignPad", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadICAdoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddICA(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadBackDocdoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddBackDoc(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadCredentialdoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddCredential(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadNDAdoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddNDA(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadLicensedoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddLicense(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult DeleteProviderAccount(int physicianid)
        {
            _providerMenu.DeleteThisAccount(physicianid);
            return RedirectToAction("Providers");
        }

        [Area("AdminArea")]
        public IActionResult CreateAccountPage()
        {
            PhysicianAccountViewModel physicianAccountViewModel = new PhysicianAccountViewModel();
            physicianAccountViewModel.regions = _db.Regions.ToList();
            physicianAccountViewModel.roles = _role.GetAllRolesToSelect();
            return View(physicianAccountViewModel);
        }

        [Area("AdminArea")]

        public IActionResult CreateProvider(PhysicianAccountViewModel physicianAccountViewModel)
        {
            var select = Request.Form["selectedregion"].ToList();
            var list = new List<int>();
            foreach (var item in select)
            {
                list.Add(int.Parse(item));
            }
            physicianAccountViewModel.SelectedRegionCB = list;
            physicianAccountViewModel.Createby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
            var physicianid = _providerMenu.AddAccount(physicianAccountViewModel);
            return RedirectToAction("AdminTabsLayout", "Home");
        }


        [Area("AdminArea")]
        [HttpPost]
        public IActionResult CreateNewShift([FromBody] CreateShiftViewModel model)
        {
            Shift shift = new Shift();
            shift.Physicianid = model.physician;
            shift.Startdate = DateOnly.Parse(model.shiftdate);
            BitArray bitArray = new BitArray(1);
            bitArray[0] = model.repeatonoff;
            shift.Isrepeat = bitArray;
            if (model.repeatdays.Count() > 0)
            {
                var weekdays = "";
                for (var i = 0; i < model.repeatdays.Count(); i++)
                {
                    weekdays = weekdays + model.repeatdays[i];
                }
                shift.Weekdays = weekdays;
            }
            shift.Repeatupto = model.repeattimes;
            shift.Createddate = DateTime.Now;
            shift.Createdby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
            _db.Shifts.Add(shift);
            _db.SaveChanges();

            var startTimeParts = model.starttime.Split(':');
            var endTimeParts = model.endtime.Split(':');
            var startTime = new TimeSpan(int.Parse(startTimeParts[0]), int.Parse(startTimeParts[1]), 0);
            var endTime = new TimeSpan(int.Parse(endTimeParts[0]), int.Parse(endTimeParts[1]), 0);

            var currentDate = shift.Startdate;
            for (int i = 0; i < model.repeattimes * 7; i++)
            {
                var currentDayOfWeek = currentDate.DayOfWeek.ToString();
                if (model.repeatdays.Contains(currentDayOfWeek))
                {
                    Shiftdetail shiftdetail = new Shiftdetail();
                    shiftdetail.Shiftid = shift.Shiftid;
                    shiftdetail.Shiftdate = currentDate;
                    shiftdetail.Starttime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startTime.Hours, startTime.Minutes, 0);
                    shiftdetail.Endtime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, endTime.Hours, endTime.Minutes, 0);
                    shiftdetail.Status = 1;
                    BitArray forfalse = new BitArray(1);
                    forfalse[0] = false;
                    shiftdetail.Isdeleted = forfalse;
                    _db.Shiftdetails.Add(shiftdetail);
                    _db.SaveChanges();
                    Shiftdetailregion shiftdetailregion = new Shiftdetailregion();
                    shiftdetailregion.Shiftdetailid = shiftdetail.Shiftdetailid;
                    shiftdetailregion.Regionid = model.region;
                    _db.Shiftdetailregions.Add(shiftdetailregion);
                    _db.SaveChanges();
                }
                currentDate = currentDate.AddDays(1);
            }


            return View();
        }



        /// <summary>
        /// Scheduling
        /// </summary>
        /// <returns></returns>

        [Area("AdminArea")]
        public IActionResult Scheduling()
        {
            return View();
        }
        [Area("AdminArea")]
        public IActionResult CreateShiftPopUp()
        {
            return PartialView("_CreateShiftPopUp");
        }


        [Area("AdminArea")]
        [HttpPost]
        public IActionResult DayWiseData(DateTime currentDate)
        {
            return PartialView("_Daywisedata");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult WeekWiseData(DateTime currentDate)
        {
            return PartialView("_Weekwisedata");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult MonthWiseData(DateTime currentDate)
        {
            return PartialView("_Monthwisedata");
        }



        /// <summary>
        /// Invoicing
        /// </summary>
        /// <returns></returns>



        [Area("AdminArea")]
        public IActionResult Invoicing()
        {
            return View();
        }

    }
}
