using DataAccess.Repository.IRepository;
using HelloDoc;

namespace DataAccess.Repository
{
    public class ShiftRepository : Repository<Shift>, IShiftRepository
    {
        public ShiftRepository(HelloDocDbContext db) : base(db)
        {
        }
    }
}
