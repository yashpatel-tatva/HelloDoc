using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using HelloDoc;
using System.Globalization;

namespace DataAccess.ServiceRepository
{
    public class PaginationRepository : IPaginationRepository
    {
        private readonly IRequestRepository _request;
        private readonly IAllRequestDataRepository _allRequestDataRepository;
        private readonly IPhysicianRepository _physician;
        public PaginationRepository(IRequestRepository requestRepository, IAllRequestDataRepository allRequestDataRepository, IPhysicianRepository physicianRepository)
        {
            _request = requestRepository;
            _allRequestDataRepository = allRequestDataRepository;
            _physician = physicianRepository;
        }
        public List<Request> requests(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        {
            var request = _request.GetRequestsbyState(state).OrderByDescending(e => e.Createddate).ToList();
            if (requesttype != 0 || requesttype == null)
            {
                request = request.Where(x => x.Requesttypeid == requesttype).ToList();
            }
            if (search != null)
            {
                search = search.Replace(" ", "");
                List<Request> requests = new List<Request>();
                if (search != null || search != "")
                {
                    foreach (var item in request)
                    {
                        string firstname = item.User.Firstname;
                        string lastname = item.User.Lastname;
                        string email = item.Requestclients.First().Email;
                        string address = item.Requestclients.First().Address;
                        string cphone = item.Requestclients.First().Phonenumber;
                        string rphone = item.Phonenumber;
                        string remail = item.Email;
                        string physician = "";
                        string phyemail = "";
                        string dob = item.User.Intdate.ToString() + "-" + DateTime.ParseExact(item.User.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month.ToString() + "_" + item.User.Intyear.ToString();
                        if (item.Physicianid != null)
                        {
                            physician = _physician.GetFirstOrDefault(x => x.Physicianid == item.Physicianid).Firstname + _physician.GetFirstOrDefault(x => x.Physicianid == item.Physicianid).Lastname;
                            phyemail = _physician.GetFirstOrDefault(x => x.Physicianid == item.Physicianid).Email;
                        }
                        string fullstring = firstname + lastname + email + address + cphone + remail + rphone + physician + phyemail + dob;
                        fullstring = fullstring.Replace(" ", "");
                        fullstring = fullstring.ToLower();
                        search = search.ToLower();
                        if (fullstring.Contains(search))
                        {
                            requests.Add(item);
                        }
                    }
                    request = requests;
                }
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

        public List<Request> requestsofProvider(string state, int currentpage, int pagesize, int requesttype, string search, int region, int providerid)
        {
            var request = new List<Request>();
            if (state == "New")
            {
                request = _request.GetRequestsbyState("New").Where(x => (x.Physicianid == providerid && x.Accepteddate == null)).ToList();
            }
            if (state == "Pending")
            {
                request = _request.GetRequestsbyState("Pending").Where(x => (x.Physicianid == providerid && x.Accepteddate != null)).ToList();
            }
            if (state == "Active")
            {
                request = _request.GetRequestsbyState("Active").Where(x => x.Physicianid == providerid).ToList();
            }
            if (state == "Conclude")
            {
                request = _request.GetRequestsbyState("Conclude").Where(x => x.Physicianid == providerid).ToList();
            }
            if (requesttype != 0 || requesttype == null)
            {
                request = request.Where(x => x.Requesttypeid == requesttype).ToList();
            }
            if (search != null)
            {
                search = search.Replace(" ", "");
                List<Request> requests = new List<Request>();
                if (search != null || search != "")
                {
                    foreach (var item in request)
                    {
                        string firstname = item.User.Firstname;
                        string lastname = item.User.Lastname;
                        string email = item.Requestclients.First().Email;
                        string address = item.Requestclients.First().Address;
                        string cphone = item.Requestclients.First().Phonenumber;
                        string rphone = item.Phonenumber;
                        string remail = item.Email;
                        string physician = "";
                        string phyemail = "";
                        string dob = item.User.Intdate.ToString() + "-" + DateTime.ParseExact(item.User.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month.ToString() + "_" + item.User.Intyear.ToString();
                        if (item.Physicianid != null)
                        {
                            physician = _physician.GetFirstOrDefault(x => x.Physicianid == item.Physicianid).Firstname + _physician.GetFirstOrDefault(x => x.Physicianid == item.Physicianid).Lastname;
                            phyemail = _physician.GetFirstOrDefault(x => x.Physicianid == item.Physicianid).Email;
                        }
                        string fullstring = firstname + lastname + email + address + cphone + remail + rphone + physician + phyemail + dob;
                        fullstring = fullstring.Replace(" ", "");
                        fullstring = fullstring.ToLower();
                        search = search.ToLower();
                        if (fullstring.Contains(search))
                        {
                            requests.Add(item);
                        }
                    }
                    request = requests;
                }
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
