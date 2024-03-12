using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    public class AdminProfileController : Controller
    {
        [Area("AdminArea")]
        public IActionResult AdminProfile()
        {
            return View();
        }
    }
}
