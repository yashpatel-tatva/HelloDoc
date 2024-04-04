using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IPhysicianRepository : IRepository<Physician>
    {
        List<Physician> getAll();
        List<Physician> getAllnotdeleted();
    }
}
