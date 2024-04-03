using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AdminPartnersTabController : Controller
    {
        private readonly HelloDocDbContext _db;

        public AdminPartnersTabController(HelloDocDbContext db)
        {
            _db = db;
        }

        [Area("AdminArea")]
        public IActionResult Vendors()
        {
            return View();
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult GetVendorsDetail(string search, int profession)
        {
            VendorsListViewModel model = new VendorsListViewModel();
            if (profession == 0)
            {
                model.healthprofessionals = _db.Healthprofessionals.ToList();
            }
            else
            {
                model.healthprofessionals = _db.Healthprofessionals.Where(x => x.Profession == profession).ToList();
            }
            model.healthprofessionaltypes = _db.Healthprofessionaltypes.ToList();
            if (search != null)
            {
                search = search.Replace(" ", "");
                List<Healthprofessional> healthprofessionals = new List<Healthprofessional>();
                if (search != null || search != "")
                {
                    foreach (var item in model.healthprofessionals)
                    {
                        var prof = model.healthprofessionaltypes.First(x => x.Healthprofessionalid == item.Profession).Professionname;
                        string fullstring = item.Vendorname + item.Faxnumber + item.City + item.State + item.Zip + item.Phonenumber + item.Email + item.Businesscontact +prof;
                        fullstring = fullstring.Replace(" ", "");
                        fullstring = fullstring.ToLower();
                        search = search.ToLower();
                        if (fullstring.Contains(search))
                        {
                            healthprofessionals.Add(item);
                        }
                    }
                    model.healthprofessionals = healthprofessionals;
                }
            }
            return PartialView("_VendorsDetail", model);
        }

        [Area("AdminArea")]
        public List<Healthprofessionaltype> GetProfessionalTypes()
        {
            List<Healthprofessionaltype> healthprofessionaltype = _db.Healthprofessionaltypes.ToList();
            return healthprofessionaltype;
        }

    }
}
