using DataAccess.Repository.IRepository;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class GroupchatRepository : Repository<Groupchat>, IGroupchatRepository
    {
        public GroupchatRepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }

        public void Addthischat(Groupchat ch)
        {
            var model = ch;
            _db.Groupchats.Add(model);
            _db.SaveChanges();
        }

        public List<Groupchat> HistoryOfthisgroup(string opp)
        {
            var gro = _db.Groupchats.Where(x=>x.Groupid.ToString() == opp).ToList();
            return gro;
        }
    }
}
