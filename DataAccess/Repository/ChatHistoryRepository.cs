using DataAccess.Repository.IRepository;
using HelloDoc;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var unread = _db.Chathistories.Where(x => x.Reciever == a && x.Isread == false && x.Sender == fromthis).ToList();
            foreach (var item in unread)
            {
                item.Isread = true;
                item.Readtime = DateTime.Now;
                _db.Chathistories.Update(item);
                _db.SaveChanges();
            }
            MsgSent(a);
        }

      
        public void MsgSent(string aspid)
        {
            var unread = _db.Chathistories.Where(x => x.Reciever == aspid && x.Isread == false).ToList();
            foreach (var item in unread)
            {
                item.Issent = true;
                item.Recievetime = DateTime.Now;
                _db.Chathistories.Update(item);
                _db.SaveChanges();
            }
        }
    }
}
