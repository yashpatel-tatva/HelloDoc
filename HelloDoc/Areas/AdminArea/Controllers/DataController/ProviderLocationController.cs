using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
            List<Physician> physician = _physician.GetAll().AsEnumerable().Where(x => x.Isdeleted == null || x.Isdeleted[0] == false).ToList();
            List<LocationData> result = new List<LocationData>();
            
            var lastLocations = _db.Physicianlocations.Include(x => x.Physician).AsEnumerable().Where(x => x.Physician.Isdeleted == null || x.Physician.Isdeleted[0]==false)
                                 .GroupBy(l => l.Physicianid)
                                 .Select(g => g.OrderByDescending(l => l.Createddate).First())
                                 .ToList();
            foreach (var phy in lastLocations)
            {
                result.Add(new LocationData() {
                    Physicianid = phy.Physicianid,
                    Name = phy.Physician.Firstname + " " + phy.Physician.Lastname,
                    Photo = phy.Physician.Photo,
                    Lat = phy.Latitude,
                    Long = phy.Longitude
                });
            }

            var providerlocation = result.ToJson();

            return Json(providerlocation);
        }
    }
}
