using DataAccess.Repository.IRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly IHttpContextAccessor _httpsession;
        private readonly IAspNetUserRepository _aspnetuser;
        public AdminRepository(IHttpContextAccessor httpContextAccessor, IAspNetUserRepository aspNetUserRepository, HelloDocDbContext db) : base(db)
        {
            _db = db;
            _httpsession = httpContextAccessor;
            _aspnetuser = aspNetUserRepository;
        }

        public void SetSession(Admin admin)
        {
            _httpsession.HttpContext.Session.SetInt32("AdminId", admin.Adminid);
            _httpsession.HttpContext.Session.SetString("AspNetId", admin.Aspnetuserid);
            _httpsession.HttpContext.Session.SetString("UserName", admin.Firstname + " " + admin.Lastname);
            _httpsession.HttpContext.Session.SetString("Role", "Admin");
        }

        public void RemoveSession()
        {
            _httpsession.HttpContext.Session.Remove("AdminId");
            _httpsession.HttpContext.Session.Remove("UserName");
            _httpsession.HttpContext.Session.Remove("AspNetId");
            _httpsession.HttpContext.Session.Remove("Role");
        }
        public int GetSessionAdminId()
        {
            try
            {
                var id = (int)_httpsession.HttpContext.Session.GetInt32("AdminId");
                return id;
            }
            catch
            {
                return -1;
            }
        }

        public AdminProfileViewModel GetAdminProfile(int id)
        {
            var admin = _db.Admins.Include(r=>r.Aspnetuser).Include(x=>x.Adminregions).FirstOrDefault(x => x.Adminid == id);
            AdminProfileViewModel model = new AdminProfileViewModel();
            model.Username = admin.Aspnetuser.Username;
            model.Password = admin.Aspnetuser.Passwordhash;
            if(admin.Status == 0)
            {
                model.Status = "Disable";
            }
            else if(admin.Status == 1)
            {
                model.Status = "Active";
            }
            //model.Role = _db.Roles.FirstOrDefault(x => x.Roleid == admin.Roleid).Name;
            model.FirstName = admin.Firstname;
            model.LastName = admin.Lastname;
            model.Email = admin.Email;
            model.Mobile = admin.Mobile;
            model.Address1 = admin.Address1;
            model.Address2 = admin.Address2;
            model.Zip = admin.Zip;
            model.Region = (int)admin.Regionid;
            model.Aspnetid = admin.Aspnetuserid;
            List<string> regionidstring = new List<string>();
            foreach (var item in admin.Adminregions)
            {
               regionidstring.Add(item.Regionid.ToString());
            }
            model.SelectRegion = regionidstring;
            return model;
        }

        public void Edit(int adminid ,AdminProfileViewModel viewModel)
        {
            var admin = _db.Admins.Include(x => x.Adminregions).FirstOrDefault(x => x.Adminid == adminid);
            var aspnet = _aspnetuser.GetFirstOrDefault(x => x.Id == admin.Aspnetuserid);
            admin.Firstname = viewModel.FirstName;
            admin.Lastname = viewModel.LastName;
            admin.Email = viewModel.Email;
            aspnet.Email = viewModel.Email;
            admin.Mobile = viewModel.Mobile;
            aspnet.Phonenumber = viewModel.Mobile;
            //admin.Address1 = viewModel.Address1;
            //admin.Address2 = viewModel.Address2;
            //admin.Zip = viewModel.Zip;
            //admin.Regionid = viewModel.Region;
            var RegionToDelete = admin.Adminregions.Select(x=>x.Regionid.ToString()).Except(viewModel.SelectRegion);
            foreach (var item in RegionToDelete)
            {
                Adminregion? adminRegionToDelete = _db.Adminregions
            .FirstOrDefault(ar => ar.Adminid == adminid && ar.Regionid.ToString() == item);

                if (adminRegionToDelete != null)
                {
                    _db.Adminregions.Remove(adminRegionToDelete);
                }
            }
            IEnumerable<string> regionsToAdd = viewModel.SelectRegion.Except(admin.Adminregions.Select(x=>x.Regionid.ToString()));

            foreach (var item in regionsToAdd)
            {
                Adminregion newAdminRegion = new Adminregion
                {
                    Adminid = (int)adminid,
                    Regionid = int.Parse(item),
                };
                _db.Adminregions.Add(newAdminRegion);
            }
            _db.Update(admin);
            _aspnetuser.Update(aspnet);
            _db.SaveChanges();
        }

        public void EditBillingDetails(int adminid, AdminProfileViewModel viewModel)
        {
            var admin = GetFirstOrDefault(x => x.Adminid == adminid);
            admin.Address1 = viewModel.Address1;
            admin.Address2 = viewModel.Address2;
            admin.Zip = viewModel.Zip;
            admin.Regionid = viewModel.Region;
            _db.Admins.Update(admin);
            _db.SaveChanges();
        }
    }
}
