using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class ProviderProfileController : Controller
    {
        private readonly IPhysicianRepository _physician;
        private readonly ISendEmailRepository _sendemail;
        private readonly IAdminRepository _admin;
        private readonly IRequestStatusLogRepository _requestStatusLog;
        private readonly HelloDocDbContext dbContext;
        public ProviderProfileController(IPhysicianRepository physician, ISendEmailRepository sendEmailRepository, IRequestStatusLogRepository requestStatusLog, HelloDocDbContext dbContext, IAdminRepository admin)
        {
            _physician = physician;
            _sendemail = sendEmailRepository;
            _requestStatusLog = requestStatusLog;
            this.dbContext = dbContext;
            _admin = admin;
        }

        [Area("ProviderArea")]
        public IActionResult ProviderProfile()
        {
            return View(new { physicianid = _physician.GetSessionPhysicianId() });
        }

        [Area("ProviderArea")]
        [HttpPost]
        public IActionResult RequesttoadminPopup(int physicianid)
        {
            return PartialView("_RequestToAdmin", new { physicianid = physicianid });
        }
        [Area("ProviderArea")]
        [HttpPost]
        public void RequesttoAdmin(int physicianid, string message)
        {
            var admin = dbContext.Requeststatuslogs.Where(x => x.Transtophysicianid == physicianid).OrderBy(x => x.Createddate).LastOrDefault().Adminid;
            var adminemail = _admin.GetFirstOrDefault(x => x.Adminid == admin).Email;
            _sendemail.Sendemail(adminemail, "Requet For Edit profle", message);
        }
    }
}
