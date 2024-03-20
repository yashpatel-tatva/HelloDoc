using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public AdminProviderTabController(IProviderMenuRepository providerMenu, ISendEmailRepository sendEmail, IPhysicianRepository physicianRepository, IAdminRepository adminRepository, IAspNetUserRepository userRepository)
        {
            _providerMenu = providerMenu;
            _sendEmail = sendEmail;
            _physician = physicianRepository;
            _admin = adminRepository;
            _userRepository = userRepository;
        }
        [Area("AdminArea")]
        public IActionResult Scheduling()
        {
            return View();
        }
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
        public void ChangeNotification(List<int> checkedToUnchecked, List<int> uncheckedToChecked)
        {
            _providerMenu.ChangeNotification(checkedToUnchecked, uncheckedToChecked);
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
            if(file != null)
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
            if(file != null)
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
            if(file != null)
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
            if(file != null)
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
        //[Area("AdminArea")]
        //[HttpPost]
        //public string ViewFile(int physicianid)
        //{
            
        //}

        [Area("AdminArea")]
        public IActionResult Invoicing()
        {
            return View();
        }

    }
}
