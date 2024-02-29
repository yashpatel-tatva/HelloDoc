using DataAccess.Repository.IRepository;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Http;
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
        public AdminRepository(IHttpContextAccessor httpContextAccessor, HelloDocDbContext db) : base(db)
        {
            _db = db;
            _httpsession = httpContextAccessor;
        }

        public void SetSession(Admin admin)
        {
            _httpsession.HttpContext.Session.SetInt32("AdminId", admin.Adminid);
            _httpsession.HttpContext.Session.SetString("UserName", admin.Firstname + " " + admin.Lastname);
        }

        public void RemoveSession()
        {
            _httpsession.HttpContext.Session.Remove("AdminId");
            _httpsession.HttpContext.Session.Remove("UserName");
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
    }
}
