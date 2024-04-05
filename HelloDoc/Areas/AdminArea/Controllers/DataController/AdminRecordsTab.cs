using DataAccess.ServiceRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AdminRecordsTab : Controller
    {

        [Area("AdminArea")]
        public IActionResult PatientHistory()
        {
            return View();
        }

        [Area("AdminArea")]
        public IActionResult PatientRecords()
        {
            return View();
        }

        [Area("AdminArea")]
        public IActionResult SearchRecords()
        {
            return View();
        }
        [Area("AdminArea")]
        public IActionResult EmailLogs()
        {
            return View();
        }
        [Area("AdminArea")]
        public IActionResult SmsLogs()
        {
            return View();
        }
        [Area("AdminArea")]
        public IActionResult BlockHistory()
        {
            return View();
        }

    }
}
