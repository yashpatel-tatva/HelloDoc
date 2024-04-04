using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AdminPartnersTabController : Controller
    {
        private readonly HelloDocDbContext _db;
        private readonly IHealthProfessionalsRepository _healthProfessionals;

        public AdminPartnersTabController(HelloDocDbContext db, IHealthProfessionalsRepository healthProfessionalsRepository)
        {
            _db = db;
            _healthProfessionals = healthProfessionalsRepository;
        }

        [Area("AdminArea")]
        public IActionResult Vendors()
        {
            return View();
        }

        [Area("AdminArea")]
        //[HttpPost]
        public IActionResult GetVendorsDetail(string search, int profession)
        {
            VendorsListViewModel model = new VendorsListViewModel();
            if (profession == 0)
            {
                model.healthprofessionals = _healthProfessionals.GetVendorsToShow();
            }
            else
            {
                model.healthprofessionals = _healthProfessionals.GetVendorsToShow().Where(x => x.Profession == profession).ToList();
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
                        string fullstring = item.Vendorname + item.Faxnumber + item.City + item.State + item.Zip + item.Phonenumber + item.Email + item.Businesscontact + prof;
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

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult DeleteThisVendor(int vendorid, int professiontype, string search)
        {
            _healthProfessionals.DeleteThisVendor(vendorid);
            return RedirectToAction("GetVendorsDetail", new { search = search, profession = professiontype });
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult VendorEditCreate(string id)
        {
            if (id == "CreateNew")
            {
                return View();
            }
            else
            {
                var vendor = _healthProfessionals.GetFirstOrDefault(x => x.Vendorid == int.Parse(id));
                VendorDetailViewModel model = new VendorDetailViewModel
                {
                    id = vendor.Vendorid,
                    Name = vendor.Vendorname,
                    Profession = (int)vendor.Profession,
                    FaxNumber = vendor.Faxnumber,
                    PhoneNumber = vendor.Phonenumber,
                    Email = vendor.Email,
                    BusinessContact = vendor.Businesscontact,
                    Street = vendor.Address,
                    State = vendor.State,
                    City = vendor.City,
                    Zip = vendor.Zip
                };
                return View(model);
            }
        }


        [Area("AdminArea")]
        [HttpPost]
        public IActionResult CreateOrEditVendor(VendorDetailViewModel model)
        {
            List<int> regionids = _db.Regions.Select(x => x.Regionid).ToList();
            Random rand = new Random();
            int index = rand.Next(regionids.Count());
            int variable = regionids[index];
            BitArray forfalse = new BitArray(1);
            forfalse[0] = false;
            if (model.id == 0)
            {
                Healthprofessional healthprofessional = new Healthprofessional
                {
                    Vendorname = model.Name,
                    Profession = (int)model.Profession,
                    Faxnumber = model.FaxNumber,
                    Phonenumber = model.PhoneNumber,
                    Email = model.Email,
                    Businesscontact = model.BusinessContact,
                    Address = model.Street,
                    State = model.State,
                    City = model.City,
                    Zip = model.Zip,
                    Isdeleted = forfalse,
                    Regionid = variable,
                    Createddate = DateTime.Now,
                };
                _db.Healthprofessionals.Add(healthprofessional);
                _db.SaveChanges();
                TempData["Message"] = "Vendor Added";
            }
            else
            {
                Healthprofessional healthprofessional = new Healthprofessional
                {
                    Vendorid = model.id,
                    Vendorname = model.Name,
                    Profession = (int)model.Profession,
                    Faxnumber = model.FaxNumber,
                    Phonenumber = model.PhoneNumber,
                    Email = model.Email,
                    Businesscontact = model.BusinessContact,
                    Address = model.Street,
                    State = model.State,
                    City = model.City,
                    Isdeleted = forfalse,
                    Zip = model.Zip,
                    Regionid = variable,
                    Modifieddate = DateTime.Now,
                };
                _db.Healthprofessionals.Update(healthprofessional);
                _db.SaveChanges();
                TempData["Message"] = "Vendor Updated";
            }
            return RedirectToAction("AdminTabsLayout", "Home");
        }
    }
}
