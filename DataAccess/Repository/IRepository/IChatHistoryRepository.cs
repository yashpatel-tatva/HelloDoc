using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IChatHistoryRepository : IRepository<Chathistory>
    {
        List<Chathistory> HistoryOfthisTwo(string sender, string receiver);
        void MsgSeen(string aspid, string fromthis);
        void MsgSent(string aspid);
    }
}
