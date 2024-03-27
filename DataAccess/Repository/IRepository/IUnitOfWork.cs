namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAspNetUserRepository AspNetUser { get; }
        IUserRepository User { get; }
        IRequestRepository Request { get; }

        void Save();
    }
}
