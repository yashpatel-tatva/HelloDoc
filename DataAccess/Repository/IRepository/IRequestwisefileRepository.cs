using HelloDoc;
using Microsoft.AspNetCore.Http;

namespace DataAccess.Repository.IRepository
{
    public interface IRequestwisefileRepository : IRepository<Requestwisefile>
    {
        void Delete(int id);

        void Add(int id, List<IFormFile> formFiles);
    }
}
