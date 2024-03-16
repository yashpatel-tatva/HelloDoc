using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository.IServiceRepository
{
    public interface IPaginationRepository
    {
        List<Request> requests(string state, int currentpage, int pagesize, int requesttype, string search, int region);
    }
}
