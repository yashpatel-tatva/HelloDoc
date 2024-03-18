using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
