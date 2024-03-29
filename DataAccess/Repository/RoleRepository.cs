using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DataAccess.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly IRoleMenuRepository _rolemenu;
        private readonly IMenuRepository _menu;
        public RoleRepository(HelloDocDbContext db, IRoleMenuRepository rolemenu, IMenuRepository menu) : base(db)
        {
            _rolemenu = rolemenu;
            _menu = menu;
        }

        public void AddThisRole(string rolename, int accounttype, List<int> menuitems, string aspnetuserid)
        {
            Role role = new Role();
            role.Name = rolename;
            role.Createddate = DateTime.Now;
            role.Createdby = aspnetuserid;
            role.Accounttype = (short)accounttype;
            BitArray isdelete = new BitArray(1);
            isdelete[0] = false;
            role.Isdeleted = isdelete;
            _db.Roles.Add(role);
            _db.SaveChanges();
            _rolemenu.AddMenusToRole(role.Roleid, menuitems);
        }

        public void DeleteThisRole(int roleid)
        {
            var role = GetFirstOrDefault(x => x.Roleid == roleid);
            role.Isdeleted[0] = true;
            _db.Roles.Update(role);
            _db.SaveChanges();
        }

        public void EditThisRole(int roleid, int accounttype, List<int> menuitems, string aspnetuserid)
        {
            var role = GetFirstOrDefault(x => x.Roleid == roleid);
            role.Modifieddate = DateTime.Now;
            role.Modifiedby = aspnetuserid;
            _db.Roles.Update(role);
            _db.SaveChanges();
            var rolemenutoadd = new List<int>();
            var rolemenutodelete = new List<int>();
            rolemenutoadd = menuitems.Except(GetMenusByRole(roleid).Select(x => x.Menuid)).ToList();
            rolemenutodelete = GetMenusByRole(roleid).Select(x => x.Menuid).Except(menuitems).ToList();
            _rolemenu.AddMenusToRole(role.Roleid, rolemenutoadd);
            _rolemenu.DeleteMenusFromRole(role.Roleid, rolemenutodelete);
        }

        public List<Role> GetAllRolesToSelect()
        {
            var roles = _db.Roles.ToList().Where(x => x.Isdeleted[0] == false).ToList();
            return roles;
        }

        public List<Menu> GetMenusByRole(int roleid)
        {
            var menus = _db.Rolemenus.Where(x => x.Roleid == roleid).Select(x => x.Menuid).ToList();
            List<Menu> result = new List<Menu>();
            foreach (var menu in menus)
            {
                var m = _menu.GetFirstOrDefault(x => x.Menuid == menu);
                result.Add(m);
            }
            return result;
        }

        public List<Menu> GetRemainingMenusByRole(int roleid)
        {
            var menus = _db.Rolemenus.Include(x => x.Menu).Where(x => x.Roleid == roleid).ToList();
            var selectedmenu = menus.Select(x => x.Menu).ToList();
            var allMenus = _db.Menus.ToList();
            var result = allMenus.Except(selectedmenu).ToList();
            return result;
        }
    }
}
