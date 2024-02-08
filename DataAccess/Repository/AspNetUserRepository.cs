using DataAccess.Repository.IRepository;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AspNetUserRepository : Repository<HelloDoc.DataModels.Aspnetuser>, IAspNetUserRepository
    {
        private HelloDocDbContext _db;
        public AspNetUserRepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Aspnetuser user)
        {
            _db.Aspnetusers.Update(user);
        }
    }
}
