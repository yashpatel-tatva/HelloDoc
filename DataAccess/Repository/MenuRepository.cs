using DataAccess.Repository.IRepository;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(HelloDocDbContext db) : base(db)
        {
        }
    }
}
