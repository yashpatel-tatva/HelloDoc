using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DataAccess.Repository
{
    public class PhysicianRepository : Repository<Physician>, IPhysicianRepository
    {
        public PhysicianRepository(HelloDocDbContext db) : base(db)
        {
            db = db;
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
            var physician = Db.Physicians.Include(r => r.Physicianregions).Include(r => r.Physiciannotifications).Include(r => r.Physicianlocations).Include(r => r.Region).Include(r => r.Requests).Include(r => r.RequeststatuslogPhysicians).Include(r => r.RequeststatuslogTranstophysicians).Include(r => r.Requestwisefiles).Where(x => x.Isdeleted == forfalse).ToList();
            return physician;
        }


    }
}
