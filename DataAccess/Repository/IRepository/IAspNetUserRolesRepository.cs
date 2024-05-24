using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IAspNetUserRolesRepository : IRepository<Aspnetuserrole>
    {
        int GetRoleFromId(string aspnetid);
    }
}
