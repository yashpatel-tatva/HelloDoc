using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class HomeController : Controller
    {
        [Area("ProviderArea")]
        public IActionResult PhysicianTabsLayout()
        {
            return View();
        }
    }
}
