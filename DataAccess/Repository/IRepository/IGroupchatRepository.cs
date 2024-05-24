using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IGroupchatRepository : IRepository<Groupchat>
    {
        void Addthischat(Groupchat ch);
        List<Groupchat> HistoryOfthisgroup(string opp);
    }
}
