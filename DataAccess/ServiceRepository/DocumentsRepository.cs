using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc;

namespace DataAccess.ServiceRepository
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private HelloDocDbContext _context;
        public readonly IRequestwisefileRepository _requestwisefilesRepository;

        public DocumentsRepository(HelloDocDbContext context, IRequestwisefileRepository requestwisefilesRepository)
        {
            _context = context;
            _requestwisefilesRepository = requestwisefilesRepository;
        }

        public void DeleteFile(int id)
        {
            _requestwisefilesRepository.Delete(id);
        }

        public byte[] Download(int id)
        {
            var path = (_context.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == id)).Filename;
            var bytes = System.IO.File.ReadAllBytes(path);
            return bytes;
        }
    }
}
