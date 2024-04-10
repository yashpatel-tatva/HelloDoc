using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class ScheduleController : Controller
    {
        [Area("ProviderArea")]
        public IActionResult Schedule()
        {
            return View();
        }
    }
}
