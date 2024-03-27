using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IAspNetUserRepository : IRepository<Aspnetuser>
    {

        bool checkpass(Aspnetuser user);
        bool checkemail(Aspnetuser user);


        void Update(Aspnetuser user);
        void Save();
        void changepass(string aspnetid, string password);
    }
}
