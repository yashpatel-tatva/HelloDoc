using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace DataAccess.Repository
{
    public class ChatHistoryRepository : Repository<Chathistory>, IChatHistoryRepository
    {
        public ChatHistoryRepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }


        public List<Chathistory> HistoryOfthisTwo(string sender, string receiver)
        {
            var hist = _db.Chathistories.Where(x => (x.Sender == sender && x.Reciever == receiver) || (x.Sender == receiver && x.Reciever == sender)).ToList();
            return hist.ToList();
        }

        public void MsgSeen(string a, string fromthis)
        {
            _db.Database.ExecuteSqlRawAsync("CALL public.msgseen(@p0, @p1)", fromthis, a);



            //var unread = _db.Chathistories.Where(x => x.Reciever == a && x.Isread == false && x.Sender == fromthis).ToList();
            //foreach (var item in unread)
            //{
            //    item.Isread = true;
            //    item.Readtime = DateTime.Now;
            //    _db.Chathistories.Update(item);
            //    _db.SaveChanges();
            //}
            //MsgSent(a);
        }


        public void MsgSent(string aspid)
        {
            //_db.Database.ExecuteSqlRawAsync("CALL public.msgsent(@p0)", aspid);

            var unread = _db.Chathistories.Where(x => x.Reciever == aspid && x.Isread == false).ToList();
            foreach (var item in unread)
            {
                if (item.Issent == false)
                {
                    item.Issent = true;
                    item.Recievetime = DateTime.Now;
                    _db.Chathistories.Update(item);
                    _db.SaveChanges();
                }
            }
        }
    }
}
