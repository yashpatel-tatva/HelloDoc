using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRoleRepository : IRepository<Role>
    {
        void AddThisRole(string rolename, int accounttype, List<int> menuitems, string aspnetuserid);
        void DeleteThisRole(int roleid);
        List<Role> GetAllRolesToSelect();
    }
}
