using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class ProviderProfileController : Controller
    {
        private readonly IPhysicianRepository _physician;
        public ProviderProfileController(IPhysicianRepository physician)
        {
            _physician = physician;
        }

        [Area("ProviderArea")]
        public IActionResult ProviderProfile()
        {
            return RedirectToAction("EditProviderPage" , "AdminProviderTab" , new {area = "AdminArea" , physicianid = _physician.GetSessionPhysicianId()});
        }
    }
}
