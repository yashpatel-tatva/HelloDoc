using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc;
using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository
{
    public class AddRequestRepository : IAddRequestRepository
    {
        private readonly HelloDocDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public AddRequestRepository(HelloDocDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public void AddPatientRequestWiseFile(List<IFormFile> formFile, int requestid)
        {
            foreach (var file in formFile)
            {
                string filename = file.FileName;
                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
                string extension = Path.GetExtension(filename);
                string filewith = filenameWithoutExtension + "_" + DateTime.Now.ToString("dd`MM`yyyy`HH`mm`ss") + extension;

                //string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HalloDoc Request Documents", requestid.ToString());
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", requestid.ToString());

                if (!Directory.Exists(directoryPath))
                {
                    // Create the directory
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, filewith);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }


                var data3 = new Requestwisefile()
                {
                    Requestid = requestid,
                    Filename = filePath,
                    Createddate = DateTime.Now,
                };

                _context.Requestwisefiles.Add(data3);

            }
            _context.SaveChanges();
        }

        public void requestfrombuisness(BusinessPartnerRequestViewModel model)
        {
            var aspnetuser = _context.Aspnetusers.FirstOrDefault(m => m.Email == model.Email);
            var user = _context.Users.FirstOrDefault(x => x.Email == model.Email);
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == user.Regionid);
            var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList(); BitArray forfalse = new BitArray(1);
            forfalse[0] = false;
            if (aspnetuser != null)
            {

                Request request = new Request
                {
                    Requesttypeid = model.Requesttypeid,
                    Userid = user.Userid,
                    Firstname = model.B_FirstName,
                    Lastname = model.B_LastName,
                    Email = model.B_Email,
                    Phonenumber = model.B_Phone,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    Casenumber = model.CaseNumber,
                    Confirmationnumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                    User = user,
                    Isurgentemailsent = forfalse
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
                    Phonenumber = model.Phone,
                    State = model.State,
                    Street = model.Street,
                    City = model.City,
                    Zipcode = model.ZipCode,
                    Address = model.Room + " , " + model.Street + " , " + model.City + " , " + model.State,
                    Intdate = model.BirthDate.Day,
                    Intyear = model.BirthDate.Year,
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Regionid = (int)user.Regionid,
                };
                _context.Add(requestclient);
                _context.SaveChanges();
                Business Business = new Business
                {
                    Name = model.B_FirstName + model.B_LastName,
                    Address1 = model.Room + " , " + model.Street + " , " + model.City + " , " + model.State,
                    City = model.City,
                    Zipcode = model.ZipCode,
                    Createddate = DateTime.Now,
                    Phonenumber = model.B_Phone,
                    Regionid = 3,
                    Createdby = aspnetuser.Id,
                };
                _context.Add(Business);
                _context.SaveChanges();
                var requestdata = _context.Requests.FirstOrDefault(m => m.Email == model.Email);
                var Businessdata = _context.Businesses.FirstOrDefault(m => m.Name == model.B_FirstName + model.B_LastName);
                Requestbusiness requestbusiness = new Requestbusiness
                {
                    Requestid = requestdata.Requestid,
                    Businessid = Business.Businessid,

                };
                _context.Add(requestbusiness);
                _context.SaveChanges();
            }
        }
        public void requestfromconcierge(ConciergeRequestViewModel model)
        {
            BitArray forfalse = new BitArray(1);
            forfalse[0] = false;
            var aspnetuser = _context.Aspnetusers.FirstOrDefault(m => m.Email == model.Email);
            var user = _context.Users.FirstOrDefault(x => x.Email == model.Email);
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == user.Regionid);
            var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
            if (aspnetuser != null)
            {
                Request request = new Request
                {
                    Requesttypeid = model.Requesttypeid,
                    Userid = user.Userid,
                    Firstname = model.C_FirstName,
                    Lastname = model.C_LastName,
                    Email = model.C_Email,
                    Phonenumber = model.C_Phone,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    Confirmationnumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                    User = user,
                    Isurgentemailsent = forfalse
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
                    Phonenumber = model.Phone,
                    State = model.C_State,
                    Street = model.C_Street,
                    City = model.C_City,
                    Zipcode = model.C_ZipCode,
                    Address = model.Room + " , " + model.C_Street + " , " + model.C_City + " , " + model.C_State,
                    Intdate = model.BirthDate.Day,
                    Intyear = model.BirthDate.Year,
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Regionid = (int)user.Regionid,
                };
                _context.Add(requestclient);
                _context.SaveChanges();
                Concierge concierge = new Concierge
                {
                    Conciergename = model.C_FirstName + model.C_LastName,
                    Address = model.Room + " , " + model.C_Street + " , " + model.C_City + " , " + model.C_State,
                    State = model.C_State,
                    Street = model.C_Street,
                    City = model.C_City,
                    Zipcode = model.C_ZipCode,
                    Createddate = DateTime.Now,
                    Regionid = 3,
                };
                _context.Add(concierge);
                _context.SaveChanges();
                Requestconcierge requestconcierge = new Requestconcierge
                {
                    Requestid = request.Requestid,
                    Conciergeid = concierge.Conciergeid,

                };
                _context.Add(requestconcierge);
                _context.SaveChanges();
            }
        }

        public void requestfromfamily(FamilyRequestViewModel model)
        {
            var aspnetuser = _context.Aspnetusers.FirstOrDefault(m => m.Email == model.Email);
            var user = _context.Users.FirstOrDefault(x => x.Email == model.Email);
            BitArray forfalse = new BitArray(1);
            forfalse[0] = false;
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == user.Regionid);
            var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
            if (aspnetuser != null)
            {
                Request request = new Request
                {
                    Requesttypeid = model.Requesttypeid,
                    Userid = user.Userid,
                    Firstname = model.F_FirstName,
                    Lastname = model.F_LastName,
                    Email = model.F_Email,
                    Phonenumber = model.F_Phone,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    Relationname = model.Relation,
                    Confirmationnumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                    User = user,
                    Isurgentemailsent = forfalse
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
                    Phonenumber = model.Phone,
                    State = model.State,
                    Street = model.Street,
                    City = model.City,
                    Zipcode = model.ZipCode,
                    Address = model.Room + " , " + model.Street + " , " + model.City + " , " + model.State,
                    Intdate = model.BirthDate.Day,
                    Intyear = model.BirthDate.Year,
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Regionid = (int)user.Regionid,
                };
                _context.Add(requestclient);
                _context.SaveChanges();
                if (model.Upload != null)
                {
                    AddPatientRequestWiseFile(model.Upload, request.Requestid);
                }
            }
        }
        public void selfrequest(PatientRequestViewModel model)
        {
            var userexist = _context.Users.FirstOrDefault(x => x.Email == model.Email);
            if (model.Password != null && userexist == null)
            {
                Aspnetuser newaspnetuser = new Aspnetuser
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = model.FirstName + model.LastName,
                    Passwordhash = model.Password,
                    Email = model.Email,
                    Createddate = DateTime.Now,
                    Phonenumber = model.PhoneNumber,
                };
                _context.Aspnetusers.Add(newaspnetuser);
                _context.SaveChanges();
                User newuser = new User
                {
                    Aspnetuserid = newaspnetuser.Id,
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Email = model.Email,
                    Mobile = model.PhoneNumber,
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
            }
            var aspnetuser = _context.Aspnetusers.FirstOrDefault(m => m.Email == model.Email);
            var user = _context.Users.FirstOrDefault(x => x.Email == model.Email);
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == user.Regionid);
            var requestcount = (from m in _context.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
            BitArray forfalse = new BitArray(1);
            forfalse[0] = false;
            if (aspnetuser != null)
            {
                Request request = new Request
                {
                    Requesttypeid = model.Requesttypeid,
                    Userid = user.Userid,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    Phonenumber = user.Mobile,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    User = user,
                    Confirmationnumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                    Isurgentemailsent = forfalse
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
                    Address = model.Room + " , " + model.Street + " , " + model.City + " , " + model.State,
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Regionid = (int)user.Regionid,
                };
                _context.Requestclients.Add(requestclient);
                _context.SaveChanges();
                if (model.Upload != null)
                {
                    AddPatientRequestWiseFile(model.Upload, request.Requestid);
                }
                int id = user.Userid;
                _contextAccessor.HttpContext.Session.SetInt32("UserId", id);
                _contextAccessor.HttpContext.Session.SetString("UserName", user.Firstname + " " + user.Lastname);
            }
        }
    }
}
