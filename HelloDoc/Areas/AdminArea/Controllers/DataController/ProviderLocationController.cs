using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class ProviderLocationController : Controller
    {
        private readonly HelloDocDbContext _db;
        private readonly IPhysicianRepository _physician;

        public ProviderLocationController(HelloDocDbContext db, IPhysicianRepository physicianRepository)
        {
            _db = db;
            _physician = physicianRepository;
        }

        [Area("AdminArea")]
        public IActionResult ProviderLocation()
        {
            return View();
        }

        [Area("AdminArea")]
        public JsonResult ProviderLocationJson()
        {
            List<Physician> physician = _physician.getAllnotdeleted();

            var providerlocation = physician.Select(x => new
            {
                Physicianid = x.Physicianid,
                Name = x.Firstname + " " + x.Lastname,
                Photo = x.Photo,
                Lat = x.Physicianlocations.LastOrDefault(x => x.Physicianid == x.Physicianid).Latitude,
                Long = x.Physicianlocations.LastOrDefault(x => x.Physicianid == x.Physicianid).Longitude
            }).ToJson();

            return Json(providerlocation);
        }
    }
}
