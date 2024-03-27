using DataAccess.Repository.IRepository;
using HelloDoc;

namespace DataAccess.Repository
{
    internal class UnitOfWork : IUnitOfWork
    {
        private HelloDocDbContext _db;
        public UnitOfWork(HelloDocDbContext db)
        {
            _db = db;
            AspNetUser = new AspNetUserRepository(_db);
            User = new UserRepository(_db);
            Request = new RequestRepository(_db);
        }
        public IAspNetUserRepository AspNetUser { get; private set; }

        public IUserRepository User { get; private set; }

        public IRequestRepository Request { get; private set; }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
