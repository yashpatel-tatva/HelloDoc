using HelloDoc.Areas.PatientArea.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IPatientFormsRepository
    {
        void AddNewUserAndAspUser(PatientRequestViewModel model);

        void AddRequestFromPatient(PatientRequestViewModel model);
    }
}
