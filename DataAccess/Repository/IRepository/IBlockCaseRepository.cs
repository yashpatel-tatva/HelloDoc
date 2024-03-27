using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IBlockCaseRepository : IRepository<Blockrequest>
    {
        void BlockRequest(int Requestid, string reason);
    }
}
