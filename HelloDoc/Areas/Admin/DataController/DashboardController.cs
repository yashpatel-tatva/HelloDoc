using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.Admin.DataController
{
    public class DashboardController : Controller
    {
        [Area("Admin")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
