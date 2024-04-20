using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IAddRequestRepository
    {
        void requestfrombuisness(BusinessPartnerRequestViewModel model);
        void selfrequest (PatientRequestViewModel model);
        void requestfromconcierge(ConciergeRequestViewModel model);
        void requestfromfamily(FamilyRequestViewModel model);
        void AddPatientRequestWiseFile(List<IFormFile> formFile, int requestid);
    }
}
