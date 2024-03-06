using DataAccess.Repository.IRepository;
using HelloDoc;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AspNetUserRepository : Repository<HelloDoc.Aspnetuser>, IAspNetUserRepository
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

        public bool checkpass(Aspnetuser user)
        {
            var admin = GetFirstOrDefault(x => x.Email == user.Email);
            if (admin.Passwordhash == user.Passwordhash) { 
                return true; ;
            }
            else
            {
                return false;
            }
        }

        public bool checkemail(Aspnetuser user) {
            var admin = GetFirstOrDefault(x => x.Email == user.Email);
            if (admin != null)
            {
                return true; ;
            }
            else
            {
                return false;
            }
        }
    }
}
