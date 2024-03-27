using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IRoleMenuRepository : IRepository<Rolemenu>
    {
        void AddMenusToRole(int roleid, List<int> menuitems);
        void DeleteMenusFromRole(int roleid, List<int> rolemenutodelete);
    }
}
