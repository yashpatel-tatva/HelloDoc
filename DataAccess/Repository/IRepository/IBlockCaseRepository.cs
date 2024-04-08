using DataModels.AdminSideViewModels;
using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IBlockCaseRepository : IRepository<Blockrequest>
    {
        void BlockRequest(int Requestid, string reason);
        List<BlockHistoryVIewModel> GetBlockHistory();

        void UnblockThis(int id);
    }
}
