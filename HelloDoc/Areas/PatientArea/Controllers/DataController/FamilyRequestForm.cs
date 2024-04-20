using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class FamilyRequestForm : Controller
    {
        public readonly IAddRequestRepository _addrequest;
        public FamilyRequestForm(IAddRequestRepository context)
        {
            _addrequest = context;
        }

        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> FamilyRequest(FamilyRequestViewModel model)
        {
            _addrequest.requestfromfamily(model);

            if (HttpContext.Session.IsAvailable)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            return RedirectToAction("Index", "Home");

        }

    }
}
