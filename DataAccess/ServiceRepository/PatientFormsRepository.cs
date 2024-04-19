using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc;
using HelloDoc.Areas.PatientArea.ViewModels;
using System.Globalization;

namespace DataAccess.ServiceRepository
{
    public class PatientFormsRepository : IPatientFormsRepository
    {
        private readonly HelloDocDbContext _context;

        public PatientFormsRepository(HelloDocDbContext context)
        {
            _context = context;
        }

        public bool AddNewUserAndAspUser(PatientRequestViewModel model)
        {
            var user = _context.Users.FirstOrDefault(x=>x.Email == model.Email);
            if (user != null)
            {
                return false;
            }
            Aspnetuser newaspnetuser = new Aspnetuser
            {
                Id = Guid.NewGuid().ToString(),
                Username = model.FirstName + model.LastName,
                Passwordhash = model.Password,
                Email = model.Email,
                Createddate = DateTime.Now,
            };
            _context.Aspnetusers.Add(newaspnetuser);
            _context.SaveChanges();
            User newuser = new User
            {
                Aspnetuserid = newaspnetuser.Id,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Street = model.Street,
                City = model.City,
                State = model.State,
                Zip = model.ZipCode,
                Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                Intdate = model.BirthDate.Day,
                Intyear = model.BirthDate.Year,
                Createdby = model.Email,
                Createddate = DateTime.Now,
                Regionid = 3,
            };
            _context.Users.Add(newuser);
            _context.SaveChanges();
            Aspnetuserrole aspnetuserrole = new Aspnetuserrole();
            aspnetuserrole.Userid = newaspnetuser.Id;
            aspnetuserrole.Roleid = "3";
            _context.Aspnetuserroles.Add(aspnetuserrole);
            _context.SaveChanges();
            return true;
        }

        public void AddRequestFromPatient(PatientRequestViewModel model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == model.Email);
            Request request = new Request
            {
                Requesttypeid = model.Requesttypeid,
                Userid = user.Userid,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Phonenumber = model.PhoneNumber,
                Status = model.Status,
                Createddate = DateTime.Now,
                User = user,
            };
            _context.Add(request);
            _context.SaveChanges();
            Requestclient requestclient = new Requestclient
            {
                Notes = model.Symptoms,
                Requestid = request.Requestid,
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Phonenumber = model.PhoneNumber,
                State = model.State,
                Street = model.Street,
                City = model.City,
                Zipcode = model.ZipCode,
                Intdate = model.BirthDate.Day,
                Intyear = model.BirthDate.Year,
                Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                Regionid = (int)user.Regionid,
            };
            _context.Requestclients.Add(requestclient);
            _context.SaveChanges();
        }
    }
}
