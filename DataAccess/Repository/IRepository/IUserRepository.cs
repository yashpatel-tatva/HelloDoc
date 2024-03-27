using HelloDoc;

namespace DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {

        void Update(User user);
        void Save();
    }
}
