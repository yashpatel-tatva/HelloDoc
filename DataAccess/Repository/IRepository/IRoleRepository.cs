using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IRoleRepository : IRepository<Role>
    {
        void AddThisRole(string rolename, int accounttype, List<int> menuitems, string aspnetuserid);
        void DeleteThisRole(int roleid);
        void EditThisRole(int rolenid, int accounttype, List<int> menuitems, string aspnetuserid);
        List<Role> GetAllRolesToSelect();

        List<Menu> GetMenusByRole(int roleid);
        List<Menu> GetRemainingMenusByRole(int roleid);
    }
}
