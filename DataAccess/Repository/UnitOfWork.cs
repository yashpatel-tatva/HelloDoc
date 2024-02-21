using DataAccess.Repository.IRepository;
using HelloDoc.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Request= new RequestRepository(_db);
        }
        public IAspNetUserRepository AspNetUser { get; private set ; }

        public IUserRepository User { get; private set ; }

        public IRequestRepository Request { get; private set ; }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
