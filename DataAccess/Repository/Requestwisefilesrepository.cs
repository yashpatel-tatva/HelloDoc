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
    public class Requestwisefilesrepository : Repository<Requestwisefile> , IRequestwisefilesRepository
    {
        public HelloDocDbContext _db;

        public Requestwisefilesrepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }
        
    }
}
