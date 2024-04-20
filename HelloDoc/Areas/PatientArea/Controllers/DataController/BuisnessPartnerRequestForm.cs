using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class BusinessPartnerRequestForm : Controller
    {
        public readonly IAddRequestRepository _addrequest;
        public BusinessPartnerRequestForm(IAddRequestRepository context)
        {
            _addrequest = context;
        }
        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> BuisneesPartnerRequest(BusinessPartnerRequestViewModel model)
        {
            _addrequest.requestfrombuisness(model);
            return RedirectToAction("Index", "Home");

        }
    }
}
