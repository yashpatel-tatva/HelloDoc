using DataAccess.Repository.IRepository;
using HelloDoc;

namespace DataAccess.Repository
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(HelloDocDbContext db) : base(db)
        {
        }
    }
}
