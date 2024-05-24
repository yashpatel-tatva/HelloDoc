using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class PatientRequestForm : Controller
    {
        public readonly IAddRequestRepository _addrequest;
        public PatientRequestForm(IAddRequestRepository context)
        {
            _addrequest = context;
        }

        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> Self(PatientRequestViewModel model)
        {
            _addrequest.selfrequest(model);

            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}
