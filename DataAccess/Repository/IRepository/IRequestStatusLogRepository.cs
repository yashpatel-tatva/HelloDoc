using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IRequestStatusLogRepository : IRepository<RequestStatusLogRepository>
    {
        void Add(Requeststatuslog requeststatuslog);

        void Update(Requeststatuslog requeststatuslog);

        List<Requeststatuslog> GetStatusbyId(int RequestId);

        void Save();
    }
}
