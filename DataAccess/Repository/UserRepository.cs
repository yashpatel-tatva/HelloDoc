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
    public class UserRepository : Repository<User>, IUserRepository
    {
        private HelloDocDbContext _db;

        public UserRepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(User user)
        {
            _db.Users.Update(user);
        }
    }
}
