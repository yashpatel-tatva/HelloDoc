using DataAccess.Repository.IRepository;
using HelloDoc;

namespace DataAccess.Repository
{
    public class AspNetUserRolesRepository : Repository<Aspnetuserrole>, IAspNetUserRolesRepository
    {
        public AspNetUserRolesRepository(HelloDocDbContext db) : base(db)
        {
            _db = db;
        }

        public int GetRoleFromId(string aspnetid)
        {
            var asprole = _db.Aspnetuserroles.FirstOrDefault(x => x.Userid == aspnetid);
            var role = asprole.Roleid;
            return int.Parse(role);
        }
    }
}
