using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DataRepository.IDataRepository
{
    public interface IDBRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        bool Any(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();

        void Add(T entity);
        void Remove(T entity);

        void RemoveRange(T entity);
        void Save();

    }
}
