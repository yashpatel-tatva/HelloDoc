using DataAccess.Repository.IRepository;
using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoleMenuRepository : Repository<Rolemenu>, IRoleMenuRepository
    {
        public RoleMenuRepository(HelloDocDbContext db) : base(db)
        {
        }

        public void AddMenusToRole(int roleid, List<int> menuitems)
        {
            foreach (var menuitem in menuitems)
            {
                Rolemenu rolemenu = new Rolemenu();
                rolemenu.Roleid = roleid;
                rolemenu.Menuid = menuitem;
                _db.Rolemenus.Add(rolemenu);
            }
            _db.SaveChanges();
        }
    }
}
