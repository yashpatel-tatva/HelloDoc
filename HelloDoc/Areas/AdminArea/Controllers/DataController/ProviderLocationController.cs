using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
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

            //var providerlocation = physician.Select(x => new
            //{
            //    Physicianid = x.Physicianid,
            //    Name = x.Firstname + " " + x.Lastname,
            //    Photo = x.Photo,
            //    Lat = x.Physicianlocations.LastOrDefault(x => x.Physicianid == x.Physicianid).Latitude,
            //    Long = x.Physicianlocations.LastOrDefault(x => x.Physicianid == x.Physicianid).Longitude
            //}).ToJson();

            List<LocationData> result = new List<LocationData>();
            foreach (var x in physician)
            {
                try
                {
                    LocationData locationdata = new LocationData();
                    locationdata.Physicianid = x.Physicianid;
                    locationdata.Name = x.Firstname + " " + x.Lastname;
                    locationdata.Photo = x.Photo;
                    locationdata.Lat = x.Physicianlocations.LastOrDefault(x => x.Physicianid == x.Physicianid).Latitude;
                    locationdata.Long = x.Physicianlocations.LastOrDefault(x => x.Physicianid == x.Physicianid).Longitude;
                    result.Add(locationdata);
                }
                catch { }
            }

            var providerlocation = result.ToJson();

            return Json(providerlocation);
        }
    }
}
