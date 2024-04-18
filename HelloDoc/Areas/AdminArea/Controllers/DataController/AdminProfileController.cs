using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AdminProfileController : Controller
    {
        private readonly IAdminRepository _admin;
        private readonly HelloDocDbContext _dbContext;
        private readonly IAspNetUserRepository _userRepository;
        private readonly IRoleRepository _role;
        public AdminProfileController(IAdminRepository admin, IAspNetUserRepository asp, HelloDocDbContext helloDocDbContext, IRoleRepository role)
        {
            _admin = admin;
            _dbContext = helloDocDbContext;
            _userRepository = asp;
            _role = role;
        }
        [Area("AdminArea")]
        public IActionResult AdminProfile()
        {
            AdminProfileViewModel model = new AdminProfileViewModel();
            model = _admin.GetAdminProfile(_admin.GetSessionAdminId());
            model.State = _dbContext.Regions.ToList();
            model.others = false;
            return View(model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ThisAdminProfile(int adminid)
        {
            AdminProfileViewModel model = new AdminProfileViewModel();
            model = _admin.GetAdminProfile(adminid);
            model.State = _dbContext.Regions.ToList();
            model.others = true;
            return PartialView("AdminProfile", model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ThisUserProfile(int userid)
        {
            PatientDashboardViewModel patientDashboard = new PatientDashboardViewModel();
            var user = _dbContext.Users.FirstOrDefault(m => m.Userid == userid);
            patientDashboard.User = user;
            DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            patientDashboard.birthdate = date;
            return PartialView("../../../PatientArea/Views/Dashboard/UserProfile", patientDashboard);
        }

        [Area("AdminArea")]
        [HttpPost]
        public void ResetPassword(string aspnetid, string password)
        {
            _userRepository.changepass(aspnetid, password);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult AdminInfoEdit(AdminProfileViewModel viewModel)
        {
            viewModel.SelectRegion = Request.Form["admineditregion"].ToList();
            _admin.Edit(viewModel.Adminid, viewModel);
            if (viewModel.others)
            {
                return RedirectToAction("UserAcess", "AccessTab");
            }
            else
            {
                return RedirectToAction("AdminProfile", "AdminProfile");
            }
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult AdminBillingInfoEdit(AdminProfileViewModel viewModel)
        {
            _admin.EditBillingDetails(viewModel.Adminid, viewModel);
            if (viewModel.others)
            {
                return RedirectToAction("UserAcess", "AccessTab");
            }
            else
            {
                return RedirectToAction("AdminProfile", "AdminProfile");
            }
        }
        [Area("AdminArea")]
        public IActionResult CreateAdmin()
        {
            AdminProfileViewModel model = new AdminProfileViewModel();
            model.State = _dbContext.Regions.ToList();
            model.roles = _role.GetAllRolesToSelect().Where(x => x.Accounttype == 1 || x.Accounttype == 0).ToList();
            return View(model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult CreateThisAdmin(AdminProfileViewModel model)
        {
            model.selectedregion = Request.Form["selectedregion"].ToList();
            _admin.CreateAdmin(model);
            TempData["Message"] = "Admin Created";
            return RedirectToAction("CreateAdmin", "AdminProfile");
        }
        [Area("AdminArea")]
        [HttpPost]
        public bool CheckEmailForAdmin(string id, string email)
        {
            var admin = _admin.GetFirstOrDefault(x => x.Aspnetuserid == id);
            var adminid = 0;
            if (admin != null)
            {
                adminid = admin.Adminid;
            }
            var check = _admin.CheckEmailExist(adminid, email);
            return check;
        }
    }
}
