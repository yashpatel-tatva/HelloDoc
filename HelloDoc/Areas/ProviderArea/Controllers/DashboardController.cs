using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class DashboardController : Controller
    {
        [Area("ProviderArea")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
