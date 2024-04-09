using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IPhysicianRepository : IRepository<Physician>
    {
        List<Physician> getAll();
        List<Physician> getAllnotdeleted();
        void SetSession(Physician physician);
        void RemoveSession();
        int GetSessionPhysicianId();
    }
}
