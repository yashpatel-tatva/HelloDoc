using DataAccess.Repository.IRepository;
using HelloDoc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly IRoleMenuRepository _rolemenu;
        public RoleRepository(HelloDocDbContext db, IRoleMenuRepository rolemenu) : base(db)
        {
            _rolemenu = rolemenu;
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
            var role = GetFirstOrDefault(x=>x.Roleid == roleid);
            role.Isdeleted[0] = true;
            _db.Roles.Update(role);
            _db.SaveChanges();
        }

        public List<Role> GetAllRolesToSelect()
        {
            var roles = _db.Roles.ToList().Where(x => x.Isdeleted[0] == false).ToList();
            return roles;
        }
    }
}
