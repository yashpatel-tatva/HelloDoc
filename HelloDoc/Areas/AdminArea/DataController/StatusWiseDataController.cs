using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.DataController
{
    public class StatusWiseDataController : Controller
    {
        private readonly IAllRequestDataRepository _allrequestdata;

        public StatusWiseDataController(IAllRequestDataRepository allrequestdata)
        {
            _allrequestdata = allrequestdata;
        }

        [Area("AdminArea")]
        public IActionResult Status_New()
        {
            var model = _allrequestdata.Status(1);
            return View(model);
        }
        [Area("AdminArea")]
        public IActionResult Status_Pending()
        {
            var model = _allrequestdata.Status(2);
            return View(model);
        }
        [Area("AdminArea")]
        public IActionResult Status_Active()
        {
            var model = _allrequestdata.Status(4).Concat(_allrequestdata.Status(5)).ToList();
            return View(model);
        }
        [Area("AdminArea")]
        public IActionResult Status_Conclude()
        {
            var model = _allrequestdata.Status(6);
            return View(model);
        }
        [Area("AdminArea")]
        public IActionResult Status_Toclose()
        {
            var model = _allrequestdata.Status(3).Concat(_allrequestdata.Status(7)).Concat(_allrequestdata.Status(8)).ToList();
            return View(model);
        }
        [Area("AdminArea")]
        public IActionResult Status_Unpaid()
        {
            var model = _allrequestdata.Status(9);
            return View(model);
        }

    }
}
