using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc;

namespace DataAccess.ServiceRepository
{
    public class PaginationRepository : IPaginationRepository
    {
        private readonly IRequestRepository _request;
        private readonly IAllRequestDataRepository _allRequestDataRepository;
        public PaginationRepository(IRequestRepository requestRepository, IAllRequestDataRepository allRequestDataRepository)
        {
            _request = requestRepository;
            _allRequestDataRepository = allRequestDataRepository;
        }
        public List<Request> requests(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        {
            var request = _request.GetRequestsbyState(state).OrderByDescending(e => e.Createddate).ToList();
            if (requesttype != 0 || requesttype == null)
            {
                request = request.Where(x => x.Requesttypeid == requesttype).ToList();
            }
            if (search != null || search == "" || search == "")
            {
                request = request.Where(a => a.User.Firstname.ToLower().Contains(search.ToLower())).ToList();
            }
            if (region != 0 || region == null)
            {
                request = request.Where(a => a.User.Regionid == region).ToList();
            }
            if (currentpage == 0 && pagesize == 0)
            {
                request = request;
            }
            else
            {
                request = request.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            }
            return request.OrderByDescending(e => e.Createddate).ToList();
        }
    }
}
