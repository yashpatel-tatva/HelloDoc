using DataAccess.Repository.IRepository;
using HelloDoc;

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

        public void DeleteMenusFromRole(int roleid, List<int> rolemenutodelete)
        {
            var rolemenusToDelete = _db.Rolemenus.Where(x => x.Roleid == roleid && rolemenutodelete.Contains(x.Menuid));

            _db.Rolemenus.RemoveRange(rolemenusToDelete);

            _db.SaveChanges();
        }
    }
}
