using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class ProviderProfileController : Controller
    {
        private readonly IPhysicianRepository _physician;
        private readonly ISendEmailRepository _sendemail;
        public ProviderProfileController(IPhysicianRepository physician , ISendEmailRepository sendEmailRepository)
        {
            _physician = physician;
            _sendemail = sendEmailRepository;
        }

        [Area("ProviderArea")]
        public IActionResult ProviderProfile()
        {
            return RedirectToAction("EditProviderPage", "AdminProviderTab", new { area = "AdminArea", physicianid = _physician.GetSessionPhysicianId() });
        }

        [Area("ProviderArea")]
        [HttpPost]
        public IActionResult RequesttoadminPopup(int physicianid)
        {
            return PartialView("_RequestToAdmin", new { physicianid = physicianid });
        }
        [Area("ProviderArea")]
        [HttpPost]
        public void RequesttoAdmin(int physicianid , string message)
        {
            var phy = _physician.GetFirstOrDefault(x => x.Physicianid== physicianid);
            var phyemail = phy.Email;
            _sendemail.Sendemail(phyemail, "Requet For Edit profle", message);
        }
    }
}
