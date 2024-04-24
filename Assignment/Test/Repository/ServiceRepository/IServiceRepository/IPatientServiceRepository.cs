using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace Repository.ServiceRepository.IServiceRepository
{
    public interface IPatientServiceRepository
    {
        public void AddPatient(PatientViewModel model);
        public bool Checkemailexist(string email, int id);
        void Deletethispatient(int id);
        void EditPatient(PatientViewModel model);
        Patient GetDataById(int id);
        List<string> GetDiesesData();
        List<string> GetDoctorData(string dieses);
        int PatientCountbyFilter(string search);
        List<Patient> PatientData(int currentpage, int pagesize, string search);
    }
}
