using DataAccess.Repository.IRepository;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RolemenuRepositorty : Repository<Rolemenu> , IRolemenuRepository
    {
        public RolemenuRepositorty(HelloDocDbContext db) : base(db)
        {
        }
    }
}
