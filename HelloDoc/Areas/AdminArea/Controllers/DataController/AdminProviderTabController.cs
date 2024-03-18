using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AdminProviderTabController : Controller
    {
        private readonly IProviderMenuRepository _providerMenu;
        public AdminProviderTabController(IProviderMenuRepository providerMenu)
        {
            _providerMenu = providerMenu;
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
            return PartialView("_ProvidersDetail" ,physicians);
        }

        [Area("AdminArea")]
        public IActionResult Invoicing()
        {
            return View();
        }

    }
}
