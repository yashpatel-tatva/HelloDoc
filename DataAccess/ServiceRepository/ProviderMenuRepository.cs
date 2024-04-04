using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DataAccess.ServiceRepository
{
    public class ProviderMenuRepository : IProviderMenuRepository
    {
        private readonly IPhysicianRepository _physician;
        private readonly IAspNetUserRepository _userRepository;
        private readonly HelloDocDbContext _db;
        public ProviderMenuRepository(IPhysicianRepository physician, HelloDocDbContext helloDocDbContext, IAspNetUserRepository aspNetUserRepository)
        {
            _physician = physician;
            _db = helloDocDbContext;
            _userRepository = aspNetUserRepository;
        }
        public List<ProviderMenuViewModel> GetAllProviderDetailToDisplay(int region, int order)
        {

            var physicians = _physician.getAll().Where(x => x.Isdeleted == null || x.Isdeleted[0] == false);
            var phyregion = _db.Physicianregions.Include(x => x.Physician).ToList();

            if (region != 0)
            {
                physicians = phyregion.Where(x => x.Regionid == region).Select(x => x.Physician).ToList();
            }
            if (order == 1)
            {
                physicians = physicians.OrderByDescending(x => x.Firstname).ToList();
            }
            else
            {
                physicians = physicians.OrderBy(x => x.Firstname).ToList();
            }
            physicians = physicians.Where(x => x.Isdeleted == null || x.Isdeleted[0] == false);
            List<ProviderMenuViewModel> providers = new List<ProviderMenuViewModel>();
            foreach (var physician in physicians)
            {
                ProviderMenuViewModel model = new ProviderMenuViewModel();
                model.PhysicanId = physician.Physicianid;
                model.Name = physician.Firstname + " " + physician.Lastname;
                model.StopNotification = physician.Physiciannotifications.ElementAt(0).Isnotificationstopped.Get(0);
                model.Role = _db.Aspnetroles.FirstOrDefault(x => x.Id == (_db.Aspnetuserroles.FirstOrDefault(x => x.Userid == physician.Aspnetuserid).Roleid)).Name;
                model.OnCallStatus = "Active";
                if (physician.Status == 0)
                    model.Status = "Not Active";
                else if (physician.Status == 1)
                    model.Status = "Active";
                else
                    model.Status = "Pending";
                model.Email = physician.Email;
                providers.Add(model);
            }
            return providers;
        }
        public void ChangeNotification(List<int> checkedToUnchecked, List<int> uncheckedToChecked)
        {
            BitArray settrue = new BitArray(1);
            settrue[0] = true;
            BitArray setfalse = new BitArray(1);
            setfalse[0] = false;

            foreach (var physician in checkedToUnchecked)
            {
                var notification = _db.Physiciannotifications.FirstOrDefault(x => x.Pysicianid == physician);
                notification.Isnotificationstopped = setfalse;
                _db.Physiciannotifications.Update(notification);
            }
            foreach (var physician in uncheckedToChecked)
            {
                var notification = _db.Physiciannotifications.FirstOrDefault(x => x.Pysicianid == physician);
                notification.Isnotificationstopped = settrue;
                _db.Physiciannotifications.Update(notification);
            }
            _db.SaveChanges();
        }

        public PhysicianAccountViewModel GetPhysicianAccountById(int Physicianid)
        {
            var physician = _db.Physicians
                    .Include(x => x.Aspnetuser)
                    .Include(r => r.Physicianregions)
                    .AsEnumerable() // Load data into memory
                    .FirstOrDefault(x => x.Physicianid == Physicianid && (x.Isdeleted == null || x.Isdeleted[0] == false));
            PhysicianAccountViewModel model = new PhysicianAccountViewModel();
            model.PhysicianId = Physicianid;
            model.Aspnetid = physician.Aspnetuserid;
            model.Username = physician.Aspnetuser.Username;
            model.Password = physician.Aspnetuser.Passwordhash;
            model.FirstName = physician.Firstname;
            model.LastName = physician.Lastname;
            model.Email = physician.Email;
            model.Phone = physician.Mobile;
            model.MedicalLicense = physician.Medicallicense;
            model.NPINumber = physician.Npinumber;
            model.SynchronizationEmail = physician.Syncemailaddress;
            List<int> regionid = new List<int>();
            foreach (var item in physician.Physicianregions)
            {
                regionid.Add(item.Regionid);
            }
            model.SelectedRegionCB = regionid;

            model.Address1 = physician.Address1;
            model.Address2 = physician.Address2;
            model.City = physician.City;
            model.RegionID = (int)physician.Regionid;
            model.Zip = physician.Zip;
            model.BusinessPhone = physician.Altphone;

            model.BusinessName = physician.Businessname;
            model.BusinessWebSite = physician.Businesswebsite;
            model.Photo = physician.Photo;
            model.Sign = physician.Signature;
            model.AdminNotes = physician.Adminnotes;

            model.IsAgreementDoc = physician.Isagreementdoc[0];
            model.IsBackgroundDoc = physician.Isbackgrounddoc[0];
            model.IsTrainingDoc = physician.Istrainingdoc[0];
            model.IsNonDisclosureDoc = physician.Isnondisclosuredoc[0];
            model.IsLicenseDoc = physician.Islicensedoc[0];
            model.IsCredentialDoc = physician.Iscredentialdoc[0];

            model.regions = _db.Regions.ToList();
            return model;
        }

        public void EditPersonalinfo(PhysicianAccountViewModel model)
        {
            var physician = _db.Physicians
                    .Include(x => x.Aspnetuser)
                    .Include(r => r.Physicianregions)
                    .AsEnumerable() // Load data into memory
                    .FirstOrDefault(x => x.Physicianid == model.PhysicianId && (x.Isdeleted == null || x.Isdeleted[0] == false));
            physician.Physicianid = model.PhysicianId;
            physician.Firstname = model.FirstName;
            physician.Lastname = model.LastName;
            physician.Email = model.Email;
            physician.Mobile = model.Phone;
            physician.Medicallicense = model.MedicalLicense;
            physician.Npinumber = model.NPINumber;
            physician.Syncemailaddress = model.SynchronizationEmail;
            var RegionToDelete = physician.Physicianregions.Select(x => x.Regionid).Except(model.SelectedRegionCB);
            foreach (var item in RegionToDelete)
            {
                Physicianregion? regiontodelete = _db.Physicianregions
            .FirstOrDefault(ar => ar.Physicianid == model.PhysicianId && ar.Regionid == item);

                if (regiontodelete != null)
                {
                    _db.Physicianregions.Remove(regiontodelete);
                }
            }
            IEnumerable<int> regionsToAdd = model.SelectedRegionCB.Except(physician.Physicianregions.Select(x => x.Regionid));

            foreach (var item in regionsToAdd)
            {
                Physicianregion newRegion = new Physicianregion
                {
                    Physicianid = model.PhysicianId,
                    Regionid = item,
                };
                _db.Physicianregions.Add(newRegion);
            }
            _db.SaveChanges();
            model.regions = _db.Regions.ToList();
            _db.Update(physician);
            _db.SaveChanges();
        }

        public void EditProviderMailingInfo(PhysicianAccountViewModel model)
        {
            var physician = _db.Physicians
                    .Include(x => x.Aspnetuser)
                    .Include(r => r.Physicianregions)
                    .AsEnumerable() // Load data into memory
                    .FirstOrDefault(x => x.Physicianid == model.PhysicianId && (x.Isdeleted == null || x.Isdeleted[0] == false));

            physician.Address1 = model.Address1;
            physician.Address2 = model.Address2;
            physician.City = model.City;
            physician.Regionid = model.RegionID;
            physician.Zip = model.Zip;
            physician.Altphone = model.BusinessPhone;

            Random rand = new Random();
            double minLat = 24.396308;
            double maxLat = 49.384358;
            double minLon = -125.000000;
            double maxLon = -66.934570;
            double latitude = minLat + (rand.NextDouble() * (maxLat - minLat));
            double longitude = minLon + (rand.NextDouble() * (maxLon - minLon));
            Physicianlocation physicianlocation = new Physicianlocation();
            physicianlocation.Physicianid = physician.Physicianid;
            physicianlocation.Latitude = (decimal?)latitude;
            physicianlocation.Longitude = (decimal?)longitude;
            physicianlocation.Createddate = DateTime.Now;
            physicianlocation.Physicianname = physician.Firstname + " " + physician.Lastname;
            physicianlocation.Address = physician.Address1 + " " + physician.City + " " + physician.Zip;
            _db.Physicianlocations.Add( physicianlocation );
            _db.Update(physician);
            _db.SaveChanges();
        }

        public void EditProviderAuthenticationInfo(PhysicianAccountViewModel model)
        {
            var physician = _db.Physicians
                    .Include(x => x.Aspnetuser)
                    .Include(r => r.Physicianregions)
                    .AsEnumerable() // Load data into memory
                    .FirstOrDefault(x => x.Physicianid == model.PhysicianId && (x.Isdeleted == null || x.Isdeleted[0] == false));

            physician.Businessname = model.BusinessName;
            physician.Businesswebsite = model.BusinessWebSite;
            physician.Photo = model.Photo;
            physician.Signature = model.Sign;
            physician.Adminnotes = model.AdminNotes;
            _db.Update(physician);
            _db.SaveChanges();
        }

        public void EditProviderAdminNote(int physicianid, string adminnote)
        {
            var physician = _db.Physicians
                    .Include(x => x.Aspnetuser)
                    .Include(r => r.Physicianregions)
                    .AsEnumerable() // Load data into memory
                    .FirstOrDefault(x => x.Physicianid == physicianid && (x.Isdeleted == null || x.Isdeleted[0] == false));

            physician.Adminnotes = adminnote;
            _db.Update(physician);
            _db.SaveChanges();
        }

        public void EditProviderPhoto(int physicianid, string base64string)
        {
            var physician = _db.Physicians
                    .Include(x => x.Aspnetuser)
                    .Include(r => r.Physicianregions)
                    .AsEnumerable() // Load data into memory
                    .FirstOrDefault(x => x.Physicianid == physicianid && (x.Isdeleted == null || x.Isdeleted[0] == false));

            physician.Photo = base64string;
            _db.Update(physician);
            _db.SaveChanges();
        }
        public void EditProviderSign(int physicianid, string base64string)
        {
            var physician = _db.Physicians
                    .Include(x => x.Aspnetuser)
                    .Include(r => r.Physicianregions)
                    .AsEnumerable() // Load data into memory
                    .FirstOrDefault(x => x.Physicianid == physicianid && (x.Isdeleted == null || x.Isdeleted[0] == false));

            physician.Signature = base64string;
            _db.Update(physician);
            _db.SaveChanges();
        }
        public void AddThisFile(int id, IFormFile file, string filename)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhysicianDocuments", id.ToString());
            string name = file.FileName;
            string extension = Path.GetExtension(name);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, filename + extension);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            BitArray fortrue = new BitArray(1); fortrue[0] = true;
            var physician = _db.Physicians
                    .Include(x => x.Aspnetuser)
                    .Include(r => r.Physicianregions)
                    .AsEnumerable() // Load data into memory
                    .FirstOrDefault(x => x.Physicianid == id && (x.Isdeleted == null || x.Isdeleted[0] == false));
            if (filename == "AgreementDoc")
            {
                physician.Isagreementdoc = fortrue;
            }
            else if (filename == "BackgroundDoc")
            {
                physician.Isbackgrounddoc = fortrue;
            }
            else if (filename == "TrainingDoc")
            {
                physician.Istrainingdoc = fortrue;
            }
            else if (filename == "NonDisclosureDoc")
            {
                physician.Isnondisclosuredoc = fortrue;
            }
            else if (filename == "LicenseDoc")
            {
                physician.Islicensedoc = fortrue;
            }
            else if (filename == "CredentialDoc")
            {
                physician.Iscredentialdoc = fortrue;
            }
            _db.Physicians.Update(physician);
            _db.SaveChanges();
        }

        public void AddICA(int physicianid, IFormFile file)
        {
            AddThisFile(physicianid, file, "AgreementDoc");
        }

        public void AddBackDoc(int physicianid, IFormFile file)
        {
            AddThisFile(physicianid, file, "BackgroundDoc");
        }

        public void AddTrainingDoc(int physicianid, IFormFile file)
        {
            AddThisFile(physicianid, file, "TrainingDoc");
        }

        public void AddNDA(int physicianid, IFormFile file)
        {
            AddThisFile(physicianid, file, "NonDisclosureDoc");
        }

        public void AddLicense(int physicianid, IFormFile file)
        {
            AddThisFile(physicianid, file, "LicenseDoc");
        }

        public void AddCredential(int physicianid, IFormFile file)
        {
            AddThisFile(physicianid, file, "CredentialDoc");
        }

        public void DeleteThisAccount(int physicianid)
        {
            var physician = _physician.GetFirstOrDefault(x => x.Physicianid == physicianid);
            BitArray b = new BitArray(1);
            b[0] = true;
            physician.Isdeleted = b;
            _db.Physicians.Update(physician);
            _db.SaveChanges();
        }

        public int AddAccount(PhysicianAccountViewModel model)
        {
            Aspnetuser aspnetuser = new Aspnetuser();
            aspnetuser.Id = Guid.NewGuid().ToString();
            aspnetuser.Username = model.Username;
            aspnetuser.Passwordhash = model.Password;
            aspnetuser.Createddate = DateTime.Now;
            aspnetuser.Email = model.Email;
            aspnetuser.Phonenumber = model.Phone;
            _db.Aspnetusers.Add(aspnetuser);
            Physician physician = new Physician();
            physician.Aspnetuserid = aspnetuser.Id;
            physician.Firstname = model.FirstName;
            physician.Lastname = model.LastName;
            physician.Email = model.Email;
            physician.Mobile = model.Phone;
            physician.Medicallicense = model.MedicalLicense;
            physician.Photo = model.Photo;
            physician.Adminnotes = model.AdminNotes;
            physician.Address1 = model.Address1;
            physician.Address2 = model.Address2;
            physician.City = model.City;
            physician.Regionid = model.RegionID;
            physician.Zip = model.Zip;
            physician.Altphone = model.BusinessPhone;
            physician.Createdby = model.Createby;
            physician.Createddate = DateTime.Now;
            physician.Status = 1;
            physician.Businessname = model.BusinessName;
            physician.Businesswebsite = model.BusinessWebSite;
            physician.Roleid = model.Roleid;
            physician.Npinumber = model.NPINumber;
            _db.Physicians.Add(physician);
            _db.SaveChanges();
            BitArray fortrue = new BitArray(1); fortrue[0] = true;
            BitArray forfalse = new BitArray(1); forfalse[0] = false;
            if (model.AgreementDoc != null)
            {
                AddICA(physician.Physicianid, model.AgreementDoc);
                physician.Isagreementdoc = fortrue;
            }
            else
            {
                physician.Isagreementdoc = forfalse;
            }
            if (model.BackgroundDoc != null)
            {
                AddBackDoc(physician.Physicianid, model.BackgroundDoc);
                physician.Isbackgrounddoc = fortrue;
            }
            else
            {
                physician.Isbackgrounddoc = forfalse;
            }
            if (model.CredentialDoc != null)
            {
                AddCredential(physician.Physicianid, model.CredentialDoc);
                physician.Iscredentialdoc = fortrue;
            }
            else
            {
                physician.Iscredentialdoc = forfalse;
            }
            if (model.NonDisclosureDoc != null)
            {
                AddNDA(physician.Physicianid, model.NonDisclosureDoc);
                physician.Isnondisclosuredoc = fortrue;
            }
            else
            {
                physician.Isnondisclosuredoc = forfalse;
            }
            physician.Istrainingdoc = forfalse;
            physician.Isdeleted = forfalse;
            physician.Islicensedoc = forfalse;
            _db.Physicians.Update(physician);
            foreach (var item in model.SelectedRegionCB)
            {
                Physicianregion physicianregion = new Physicianregion();
                physicianregion.Physicianid = physician.Physicianid;
                physicianregion.Regionid = item;
                _db.Physicianregions.Add(physicianregion);
            }
            Physiciannotification physiciannotification = new Physiciannotification();
            physiciannotification.Pysicianid = physician.Physicianid;
            physiciannotification.Isnotificationstopped = forfalse;
            _db.Physiciannotifications.Add(physiciannotification);
            Aspnetuserrole physicirole = new Aspnetuserrole();
            physicirole.Userid = aspnetuser.Id;
            physicirole.Roleid = "2";
            _db.Aspnetuserroles.Add(physicirole);
            _db.SaveChanges();
            Random rand = new Random();
            double minLat = 24.396308;
            double maxLat = 49.384358;
            double minLon = -125.000000;
            double maxLon = -66.934570;
            double latitude = minLat + (rand.NextDouble() * (maxLat - minLat));
            double longitude = minLon + (rand.NextDouble() * (maxLon - minLon));
            Physicianlocation physicianlocation = new Physicianlocation();
            physicianlocation.Physicianid = physician.Physicianid;
            physicianlocation.Latitude = (decimal?)latitude;
            physicianlocation.Longitude = (decimal?)longitude;
            physicianlocation.Createddate = DateTime.Now;
            physicianlocation.Physicianname = physician.Firstname + " " + physician.Lastname;
            physicianlocation.Address = physician.Address1 + " " + physician.City + " " + physician.Zip;
            _db.Physicianlocations.Add(physicianlocation);
            return physician.Physicianid;
        }
    }
}



