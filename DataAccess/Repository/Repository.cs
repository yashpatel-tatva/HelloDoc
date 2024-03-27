using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public HelloDocDbContext _db;
        public DbSet<T> dbSet;
        private readonly IHttpContextAccessor _httpsession;

        public HelloDocDbContext Db { get; }

        public Repository(IHttpContextAccessor httpContextAccessor)
        {
            _httpsession = httpContextAccessor;
        }

        public Repository(HelloDocDbContext db)
        {
            _db = db;
            Db = db;
            this.dbSet = Db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
            throw new NotImplementedException();
        }

        public void RemoveRange(T entity)
        {
            dbSet.RemoveRange(entity);
        }

        bool IRepository<T>.Any(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            return query.Any(filter);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
