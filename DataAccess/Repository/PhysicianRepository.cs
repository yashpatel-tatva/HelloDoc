using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DataAccess.Repository
{
    public class PhysicianRepository : Repository<Physician>, IPhysicianRepository
    {
        private readonly IHttpContextAccessor _httpsession;
        private readonly IAspNetUserRepository _aspnetuser;
        public PhysicianRepository(IHttpContextAccessor httpContextAccessor, IAspNetUserRepository aspNetUserRepository, HelloDocDbContext db) : base(db)
        {
            db = db;
            _httpsession = httpContextAccessor;
            _aspnetuser = aspNetUserRepository;
        }

        public bool CheckEmailExist(int id, string email)
        {
            if (id == 0)
            {
                var phy = GetFirstOrDefault(x => x.Email == email);
                if (phy != null)
                {
                    return true;
                }
                else { return false; }
            }
            else
            {
                var phys = GetAll().Where(x => x.Physicianid != id);
                phys = phys.Where(x=>x.Email == email);
                if (phys.Count() != 0)
                {
                    return true;
                }
                else { return false; }
            }
        }

        public List<Physician> getAll()
        {
            var physician = Db.Physicians.Include(r => r.Physicianregions).Include(r => r.Physiciannotifications).Include(r => r.Physicianlocations).Include(r => r.Region).Include(r => r.Requests).Include(r => r.RequeststatuslogPhysicians).Include(r => r.RequeststatuslogTranstophysicians).Include(r => r.Requestwisefiles).ToList();
            return physician;
        }

        public List<Physician> getAllnotdeleted()
        {
            BitArray forfalse = new BitArray(1);
            forfalse[0] = false;
            var physician = Db.Physicians.Include(r => r.Physicianregions).Include(r => r.Physiciannotifications).Include(r => r.Physicianlocations).Include(r => r.Region).Include(r => r.Requests).Include(r => r.RequeststatuslogPhysicians).Include(r => r.RequeststatuslogTranstophysicians).Include(r => r.Requestwisefiles).Include(x=>x.Physicianregions).Where(x => x.Isdeleted == forfalse).ToList();
            return physician;
        }

        public int GetSessionPhysicianId()
        {
            try
            {
                var id = (int)_httpsession.HttpContext.Session.GetInt32("PhysicianId");
                return id;
            }
            catch
            {
                return -1;
            }
        }

        public void RemoveSession()
        {
            _httpsession.HttpContext.Session.Remove("PhysicianId");
            _httpsession.HttpContext.Session.Remove("AspNetId");
            _httpsession.HttpContext.Session.Remove("UserName");
            _httpsession.HttpContext.Session.Remove("Role");
        }

        public void SetSession(Physician physician)
        {
            _httpsession.HttpContext.Session.SetInt32("PhysicianId", physician.Physicianid);
            _httpsession.HttpContext.Session.SetString("AspNetId", physician.Aspnetuserid);
            _httpsession.HttpContext.Session.SetString("UserName", physician.Firstname + " " + physician.Lastname);
            _httpsession.HttpContext.Session.SetString("Role", "Physician");
        }
    }
}
