using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class ConciergeRequestForm : Controller
    {
        public readonly IAddRequestRepository _addrequest;

        public ConciergeRequestForm(IAddRequestRepository context)
        {
            _addrequest = context;
        }
        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> Conciergerequest(ConciergeRequestViewModel model)
        {
            _addrequest.requestfromconcierge(model);
            return RedirectToAction("Index", "Home");

        }
    }
}
