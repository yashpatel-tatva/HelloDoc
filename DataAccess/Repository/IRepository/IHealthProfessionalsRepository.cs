using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IHealthProfessionalsRepository : IRepository<Healthprofessional>
    {
        public void DeleteThisVendor(int id);
        public List<Healthprofessional> GetVendorsToShow();
    }
}

