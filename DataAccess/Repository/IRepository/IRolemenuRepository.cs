using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRoleMenuRepository : IRepository<Rolemenu>
    {
        void AddMenusToRole(int roleid, List<int> menuitems);
    }
}
