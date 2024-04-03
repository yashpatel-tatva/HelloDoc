using DataAccess.ServiceRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class ProviderLocationController : Controller
    {
        [Area("AdminArea")]
        public IActionResult ProviderLocation()
        {
            return View();
        }
    }
}
