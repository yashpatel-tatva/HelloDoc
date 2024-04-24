using Entity.ViewModels;
using Repository.ServiceRepository.IServiceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace Repository.ServiceRepository
{
    public class PatientServiceRepository : IPatientServiceRepository
    {
        private readonly HMSDbContext _context;
        public PatientServiceRepository(HMSDbContext context)
        {
            _context = context;
        }
        public void AddPatient(PatientViewModel model)
        {
            var doctor = _context.Doctors.FirstOrDefault(x => x.Specialist.ToLower() == model.Specialist.ToLower());
            if (doctor == null)
            {
                Doctor newdoctor = new Doctor();
                newdoctor.Specialist = model.Specialist;
                _context.Doctors.Add(newdoctor);
                _context.SaveChanges();
                doctor = newdoctor;
            }
            Patient patient = new Patient();
            patient.FirstName = model.FirstName;
            patient.LastName = model.LastName;
            patient.DocterId = doctor.DoctorId;
            patient.Age = model.Age;
            patient.Email = model.Email;
            patient.Phone = model.Phone;
            patient.Gender = model.Gender;
            patient.Dieses = model.Dieses;
            patient.Specialist = model.Specialist;
            patient.Isdeleted = false;
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }
        public void EditPatient(PatientViewModel model)
        {
            var doctor = _context.Doctors.FirstOrDefault(x => x.Specialist.ToLower() == model.Specialist.ToLower());
            if (doctor == null)
            {
                Doctor newdoctor = new Doctor();
                newdoctor.Specialist = model.Specialist;
                _context.Doctors.Add(newdoctor);
                _context.SaveChanges();
                doctor = newdoctor;
            }
            Patient patient = _context.Patients.FirstOrDefault(x => x.Id == model.Id);
            patient.FirstName = model.FirstName;
            patient.LastName = model.LastName;
            patient.DocterId = doctor.DoctorId;
            patient.Age = model.Age;
            patient.Email = model.Email;
            patient.Phone = model.Phone;
            patient.Gender = model.Gender;
            patient.Dieses = model.Dieses;
            patient.Specialist = model.Specialist;
            patient.Isdeleted = false;
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

        public bool Checkemailexist(string email, int id)
        {
            if (id == 0)
            {
                return _context.Patients.Where(x => x.Isdeleted == false).Any(x => x.Email == email);
            }
            else
            {
                return _context.Patients.Where(x => x.Isdeleted == false).Where(x => x.Id != id).Any(x => x.Email == email);
            }
        }

        public void Deletethispatient(int id)
        {
            var patient = _context.Patients.FirstOrDefault(x => x.Id == id);
            patient.Isdeleted = true;
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }


        public Patient GetDataById(int id)
        {
            return _context.Patients.FirstOrDefault(x => x.Id == id);
        }

        public int PatientCountbyFilter(string search)
        {
            if (search == null)
            {
                return _context.Patients.Where(x => x.Isdeleted == false).Count();
            }
            //var patientlist = _context.Patients.Where(x => x.Isdeleted == false).Where(x => x.FirstName.ToLower().Contains(search)).Count();
            List<Patient> patientlist = new List<Patient>();
            foreach (var pat in _context.Patients)
            {
                var full = "";
                full = full + pat.Id.ToString() + pat.FirstName.ToLower() + pat.LastName.ToLower() + pat.Age.ToString() + pat.Email.ToLower() + pat.Phone + pat.Gender.ToLower() + pat.Dieses.ToLower() + pat.Specialist.ToLower();
                if (full.Contains(search))
                {
                    patientlist.Add(pat);
                }
            }
            return patientlist.Count();
        }

        public List<Patient> PatientData(int currentpage, int pagesize, string search)
        {
            if (search == null)
            {
                return _context.Patients.Where(x => x.Isdeleted == false).Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            }
            List<Patient> patientlist = new List<Patient>();
            foreach(var pat in _context.Patients)
            {
                var full = "";
                full = full + pat.Id.ToString() + pat.FirstName.ToLower() + pat.LastName.ToLower() + pat.Age.ToString() + pat.Email.ToLower() + pat.Phone + pat.Gender.ToLower() + pat.Dieses.ToLower() + pat.Specialist.ToLower();
                if (full.Contains(search))
                {
                    patientlist.Add(pat);
                }
            }

            //var patientlist = _context.Patients.Where(x => x.Isdeleted == false).Where(x => x.FirstName.ToLower().Contains(search)).ToList();
            patientlist = patientlist.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            return patientlist.OrderBy(x => x.Id).ToList();
        }

        public List<string> GetDiesesData()
        {
            var dieses = _context.Patients.Select(x => x.Dieses).Distinct().ToList();
            return dieses;
        }

        public List<string> GetDoctorData(string dieses)
        {
            if (dieses != null)
            {
                var doctor = _context.Patients.Where(x => x.Dieses.ToLower() == dieses.ToLower()).Select(x => x.Specialist).Distinct().ToList();
                return doctor;
            }
            return null;
        }
    }
}
