using HelloDoc.Areas.PatientArea.ViewModels;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IPatientFormsRepository
    {
        bool AddNewUserAndAspUser(PatientRequestViewModel model);

        void AddRequestFromPatient(PatientRequestViewModel model);
    }
}
