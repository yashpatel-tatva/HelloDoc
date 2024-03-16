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
        public AdminProfileController(IAdminRepository admin, IAspNetUserRepository asp, HelloDocDbContext helloDocDbContext)
        {
            _admin = admin;
            _dbContext = helloDocDbContext;
            _userRepository = asp;
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
    }
}
