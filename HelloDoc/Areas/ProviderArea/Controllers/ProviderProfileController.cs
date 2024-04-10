using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class ProviderProfileController : Controller
    {

        [Area("ProviderArea")]
        public IActionResult ProviderProfile()
        {
            return View();
        }
    }
}
