using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class InvoiceController : Controller
    {
        [Area("ProviderArea")]
        public IActionResult Invoice()
        {
            return View();
        }
    }
}
