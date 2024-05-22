using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IRequestStatusLogRepository : IRepository<Requeststatuslog>
    {
        void Add(Requeststatuslog requeststatuslog);

        void Update(Requeststatuslog requeststatuslog);

        List<Requeststatuslog> GetStatusbyId(int RequestId);
        List<Requeststatuslog> GetForPhysician(int RequestId);

        void Save();
    }
}
