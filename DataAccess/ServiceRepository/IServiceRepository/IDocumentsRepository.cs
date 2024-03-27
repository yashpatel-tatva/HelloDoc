namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IDocumentsRepository
    {
        void DeleteFile(int id);
        public byte[] Download(int id);
    }
}
