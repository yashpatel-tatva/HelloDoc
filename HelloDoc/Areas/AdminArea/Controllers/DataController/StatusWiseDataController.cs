using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.DataController
{
    [AuthorizationRepository("Admin")]
    public class StatusWiseDataController : Controller
    {
        private readonly IAllRequestDataRepository _allrequestdata;
        private readonly IPaginationRepository _paginator;
        private readonly IRequestRepository _request;

        public StatusWiseDataController(IAllRequestDataRepository allrequestdata, IPaginationRepository paginator, IRequestRepository requestRepository)
        {
            _allrequestdata = allrequestdata;
            _paginator = paginator;
            _request = requestRepository;
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult StatuswiseData(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        {
            List<Request> model1 = _paginator.requests(state, currentpage, pagesize, requesttype, search, region);
            List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
            return View("Status_" + state, filtereddata);
        }


        //[Area("AdminArea")]
        //public IActionResult Status_New(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        //{
        //    List<Request> model1 = _paginator.requests(state, currentpage, pagesize, requesttype, search, region);
        //    List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
        //    return View("Status_" + state, filtereddata);
        //}
        //[Area("AdminArea")]
        //public IActionResult Status_Pending(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        //{
        //    List<Request> model1 = _paginator.requests(state, currentpage, pagesize, requesttype, search, region);
        //    List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
        //    return View(filtereddata);
        //}
        //[Area("AdminArea")]
        //public IActionResult Status_Active(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        //{
        //    List<Request> model1 = _paginator.requests(state, currentpage, pagesize, requesttype, search, region);
        //    List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
        //    return View(filtereddata);
        //}
        //[Area("AdminArea")]
        //public IActionResult Status_Conclude(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        //{
        //    List<Request> model1 = _paginator.requests(state, currentpage, pagesize, requesttype, search, region);
        //    List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
        //    return View(filtereddata);
        //}
        //[Area("AdminArea")]
        //public IActionResult Status_Toclose(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        //{
        //    List<Request> model1 = _paginator.requests(state, currentpage, pagesize, requesttype, search, region);
        //    List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
        //    return View(filtereddata);
        //}
        //[Area("AdminArea")]
        //public IActionResult Status_Unpaid(string state, int currentpage, int pagesize, int requesttype, string search, int region)
        //{
        //    List<Request> model1 = _paginator.requests(state, currentpage, pagesize, requesttype, search, region);
        //    List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
        //    return View(filtereddata);
        //}

        [Area("AdminArea")]
        [HttpPost]
        public int CountbyFilter(string state, int requesttype, string search, int region)
        {
            List<Request> model1 = _paginator.requests(state, 0, 0, requesttype, search, region);
            return model1.Count();
        }

    }
}
