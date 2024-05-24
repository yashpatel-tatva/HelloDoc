using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Http;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IAddRequestRepository
    {
        void requestfrombuisness(BusinessPartnerRequestViewModel model);
        void selfrequest(PatientRequestViewModel model);
        void requestfromconcierge(ConciergeRequestViewModel model);
        void requestfromfamily(FamilyRequestViewModel model);
        void AddPatientRequestWiseFile(List<IFormFile> formFile, int requestid);
    }
}
