using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IPhysicianRepository _physician;
        public InvoiceController(IPhysicianRepository physician)
        {
            _physician = physician;
        }
        [Area("ProviderArea")]
        public IActionResult Invoice()
        {
            return View(new { physicianid = _physician.GetSessionPhysicianId() });
        }
    }
}
