using System;
using System.Collections;
using DataAccess.Repository.IRepository;
using HelloDoc;

namespace DataAccess.Repository
{
	public class HealthProfessionalsRepository : Repository<Healthprofessional> , IHealthProfessionalsRepository
	{
		public HealthProfessionalsRepository(HelloDocDbContext dbContext) : base(dbContext)
		{
			_db = dbContext;
		}

        public void DeleteThisVendor(int id)
        {
			var hp = GetFirstOrDefault(x => x.Vendorid == id);
			hp.Isdeleted[0] = true;
			hp.Modifieddate = DateTime.Now;
			_db.Healthprofessionals.Update(hp);
			_db.SaveChanges();
        }

        public List<Healthprofessional> GetVendorsToShow()
        {
			BitArray forfalse = new BitArray(1);
			forfalse[0] = false;
            var hp = _db.Healthprofessionals.Where(x=>x.Isdeleted == forfalse).ToList();
			return hp;       }
    }
}		

