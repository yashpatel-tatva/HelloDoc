using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;

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
            return View(model);
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
            _admin.Edit(_admin.GetSessionAdminId(), viewModel);
            return RedirectToAction("AdminTabsLayout", "Home");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult AdminBillingInfoEdit(AdminProfileViewModel viewModel)
        {
            _admin.EditBillingDetails(_admin.GetSessionAdminId(), viewModel);
            return RedirectToAction("AdminTabsLayout", "Home");
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
            return RedirectToAction("AdminTabsLayout", "Home");
        }
        [Area("AdminArea")]
        [HttpPost]
        public bool CheckEmailForAdmin(string id, string email)
        {
            var admin = _admin.GetFirstOrDefault(x => x.Aspnetuserid == id);
            var adminid = 0;
            if(admin != null)
            {
                adminid = admin.Adminid;
            }
            var check = _admin.CheckEmailExist(adminid, email);
            return check;
        }
    }
}
