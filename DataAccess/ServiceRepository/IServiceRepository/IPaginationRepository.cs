using HelloDoc;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IPaginationRepository
    {
        List<Request> requests(string state, int currentpage, int pagesize, int requesttype, string search, int region);
        List<Request> requestsofProvider(string state, int currentpage, int pagesize, int requesttype, string search, int region, int providerid);
    }
}
