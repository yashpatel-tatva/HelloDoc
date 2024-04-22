using DataAccess.Repository.IRepository;
using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin,Physician")]
    public class AdminProviderTabController : Controller
    {
        private readonly IProviderMenuRepository _providerMenu;
        private readonly ISendEmailRepository _sendEmail;
        private readonly IPhysicianRepository _physician;
        private readonly IAdminRepository _admin;
        private readonly IAspNetUserRepository _userRepository;
        private readonly IRoleRepository _role;
        private readonly IShiftDetailRepository _shiftDetail;
        private readonly IShiftRepository _shift;
        private readonly ISchedulingRepository _scheduling;
        private readonly HelloDocDbContext _db;
        public AdminProviderTabController(IProviderMenuRepository providerMenu, IShiftRepository shiftRepository, ISendEmailRepository sendEmail, IPhysicianRepository physicianRepository, IShiftDetailRepository shiftDetailRepository, IAdminRepository adminRepository, IAspNetUserRepository userRepository, HelloDocDbContext helloDocDbContext, IRoleRepository role, ISchedulingRepository scheduling)
        {
            _providerMenu = providerMenu;
            _sendEmail = sendEmail;
            _physician = physicianRepository;
            _admin = adminRepository;
            _userRepository = userRepository;
            _db = helloDocDbContext;
            _role = role;
            _shiftDetail = shiftDetailRepository;
            _shift = shiftRepository;
            _scheduling = scheduling;
        }

        /// <summary>
        /// Providers
        /// </summary>
        /// <returns></returns>

        [Area("AdminArea")]
        public IActionResult Providers()
        {
            return View();
        }

        [Area("AdminArea")]
        public IActionResult Providersfilter(int region, int order)
        {
            var physicians = _providerMenu.GetAllProviderDetailToDisplay(region, order);
            return PartialView("_ProvidersDetail", physicians);
        }
        [Area("AdminArea")]
        [HttpPost]
        public void ChangeNotification(List<int> checkedToUnchecked, List<int> uncheckedToChecked)
        {
            _providerMenu.ChangeNotification(checkedToUnchecked, uncheckedToChecked);
            // return RedirectToAction("Dashboard", "Dashboard");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ContactProviderPopUp(int physicianid)
        {
            return PartialView("_ContactProvider", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public void ContactThisProvider(int physicianid, string msgtype, string msg)
        {
            if (msgtype == "typeemail" || msgtype == "typeboth")
            {
                _sendEmail.Sendemail(_physician.GetFirstOrDefault(x => x.Physicianid == physicianid).Email, "Message From" + _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Firstname + "(Admin)", msg);
            }
        }


        [Area("AdminArea")]
        //[HttpPost]
        public IActionResult EditProviderPage(int physicianid)
        {
            PhysicianAccountViewModel model = new PhysicianAccountViewModel();
            model = _providerMenu.GetPhysicianAccountById(physicianid);
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            if (roleClaim != null)
            {
                var role = roleClaim.Value;
                model.loggedpersonrole = role;
            }
            return View(model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public void ProviderResetPassword(string aspnetid, string password)
        {
            _userRepository.changepass(aspnetid, password);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderPersonal([FromBody] PhysicianAccountViewModel viewModel)
        {
            _providerMenu.EditPersonalinfo(viewModel);
            return RedirectToAction("EditProviderPage", new { physicianid = viewModel.PhysicianId });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderMailingInfo([FromBody] PhysicianAccountViewModel viewModel)
        {
            _providerMenu.EditProviderMailingInfo(viewModel);
            return RedirectToAction("EditProviderPage", new { physicianid = viewModel.PhysicianId });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderAuthenticationInfo([FromBody] PhysicianAccountViewModel viewModel)
        {
            _providerMenu.EditProviderAuthenticationInfo(viewModel);
            return RedirectToAction("EditProviderPage", new { physicianid = viewModel.PhysicianId });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderAdminNote(int physicianid, string adminnote, string businessname, string businessweb)
        {
            _providerMenu.EditProviderAdminNote(physicianid, adminnote);
            _providerMenu.EditProviderBuisnessname_web(physicianid, businessname, businessweb);
            return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderPhoto(int physicianid, string base64string)
        {
            _providerMenu.EditProviderPhoto(physicianid, base64string);
            return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditProviderSign(int physicianid, string base64string)
        {
            _providerMenu.EditProviderSign(physicianid, base64string);
            return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult OpenSignPadPopUp(int physicianid)
        {
            return PartialView("_SignPad", new { physicianid = physicianid });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadICAdoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddICA(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadBackDocdoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddBackDoc(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadCredentialdoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddCredential(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadNDAdoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddNDA(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadLicensedoc(int physicianid, IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Only PDF files are allowed.");
                }
                _providerMenu.AddLicense(physicianid, file);
                return RedirectToAction("EditProviderPage", new { physicianid = physicianid });
            }

            return BadRequest("No file uploaded.");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult DeleteProviderAccount(int physicianid)
        {
            _providerMenu.DeleteThisAccount(physicianid);
            return RedirectToAction("Providers");
        }

        [Area("AdminArea")]
        public IActionResult CreateAccountPage()
        {
            PhysicianAccountViewModel physicianAccountViewModel = new PhysicianAccountViewModel();
            physicianAccountViewModel.regions = _db.Regions.ToList();
            physicianAccountViewModel.roles = _role.GetAllRolesToSelect().Where(x => x.Accounttype == 2 || x.Accounttype == 0).ToList();
            return View(physicianAccountViewModel);
        }

        [Area("AdminArea")]

        public IActionResult CreateProvider(PhysicianAccountViewModel physicianAccountViewModel)
        {
            var select = Request.Form["selectedregion"].ToList();
            var list = new List<int>();
            foreach (var item in select)
            {
                list.Add(int.Parse(item));
            }
            physicianAccountViewModel.SelectedRegionCB = list;
            physicianAccountViewModel.Createby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
            var physicianid = _providerMenu.AddAccount(physicianAccountViewModel);
            return RedirectToAction("Dashboard", "Dashboard");
        }



        [Area("AdminArea")]
        [HttpPost]
        public IActionResult CreateNewShift([FromBody] CreateShiftViewModel model)
        {
            Shift shift = new Shift();
            shift.Physicianid = model.physician;
            shift.Startdate = DateOnly.Parse(model.shiftdate);
            BitArray bitArray = new BitArray(1);
            bitArray[0] = model.repeatonoff;
            shift.Isrepeat = bitArray;
            if (model.repeatdays.Count() > 0)
            {
                var weekdays = "";
                for (var i = 0; i < model.repeatdays.Count(); i++)
                {
                    weekdays = weekdays + model.repeatdays[i];
                }
                shift.Weekdays = weekdays;
            }
            shift.Repeatupto = model.repeattimes;
            shift.Createddate = DateTime.Now;
            if (_admin.GetSessionAdminId() != -1)
            {
                shift.Createdby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
            }
            if (_physician.GetSessionPhysicianId() != -1)
            {
                shift.Createdby = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId()).Aspnetuserid;
            }
            _db.Shifts.Add(shift);
            _db.SaveChanges();

            var currentDate = shift.Startdate;
            var startTimeParts = model.starttime.Split(':');
            var endTimeParts = model.endtime.Split(':');
            var startTime = new TimeSpan(int.Parse(startTimeParts[0]), int.Parse(startTimeParts[1]), 0);
            var endTime = new TimeSpan(int.Parse(endTimeParts[0]), int.Parse(endTimeParts[1]), 0);
            var StartTimewithdate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startTime.Hours, startTime.Minutes, 0);
            var EndTimewithdate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, endTime.Hours, endTime.Minutes, 0);
            var shownmsg = true;



            if (shift.Weekdays == null)
            {
                bool IsValid = false;
                var shiftdetailexistdata = _db.Shiftdetails.Include(x => x.Shift).Where(x => x.Shift.Physicianid == model.physician).Where(x=>x.Shiftdate == currentDate).AsEnumerable().Where(x => x.Isdeleted[0] == false).ToList();
                if (shiftdetailexistdata.Count() != 0)
                {
                    foreach (var item in shiftdetailexistdata)
                    {
                        if ((item.Starttime > StartTimewithdate && item.Starttime > EndTimewithdate) || (item.Endtime < StartTimewithdate && item.Endtime < EndTimewithdate))
                        {
                            IsValid = true;
                        }
                        else
                        {
                            IsValid = false;
                            shownmsg = false;
                            break;
                        }

                    }
                }
                else
                {
                    IsValid = true;
                }
                if (IsValid)
                {
                    Shiftdetail shiftdetail = new Shiftdetail();
                    shiftdetail.Shiftid = shift.Shiftid;
                    shiftdetail.Shiftdate = currentDate;
                    shiftdetail.Starttime = StartTimewithdate;
                    shiftdetail.Endtime = EndTimewithdate;
                    shiftdetail.Status = 1;
                    shiftdetail.Regionid = model.region;
                    BitArray forfalse = new BitArray(1);
                    forfalse[0] = false;
                    shiftdetail.Isdeleted = forfalse;
                    _db.Shiftdetails.Add(shiftdetail);
                    _db.SaveChanges();
                    Shiftdetailregion shiftdetailregion = new Shiftdetailregion();
                    shiftdetailregion.Shiftdetailid = shiftdetail.Shiftdetailid;
                    shiftdetailregion.Regionid = model.region;
                    shiftdetailregion.Isdeleted = forfalse;
                    _db.Shiftdetailregions.Add(shiftdetailregion);
                    _db.SaveChanges();
                    TempData["Message"] = "Shift Created for : " + _physician.GetFirstOrDefault(x => x.Physicianid == shift.Physicianid).Firstname;
                }
            }
            for (int i = 0; i < model.repeattimes * 7; i++)
            {
                var IsValid = false;
                StartTimewithdate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startTime.Hours, startTime.Minutes, 0);
                EndTimewithdate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, endTime.Hours, endTime.Minutes, 0);
                var shiftdetailexistdata = _db.Shiftdetails.Include(x => x.Shift).Where(x => x.Shift.Physicianid == model.physician).Where(x => x.Shiftdate == currentDate).AsEnumerable().Where(x => x.Isdeleted[0]==false).ToList();
                if (shiftdetailexistdata.Count() != 0)
                {
                    foreach (var s in shiftdetailexistdata)
                    {
                        if ((s.Starttime > StartTimewithdate && s.Starttime > EndTimewithdate) || (s.Endtime < StartTimewithdate && s.Endtime < EndTimewithdate))
                        {
                            shownmsg = true;
                            IsValid = true;
                        }
                        else
                        {
                            IsValid = false;
                            shownmsg = false;
                            break;
                        }
                    }
                }
                else
                {
                    IsValid = true;
                }
                if (IsValid)
                {
                    var currentDayOfWeek = ((int)currentDate.DayOfWeek).ToString();
                    if (model.repeatdays.Contains(currentDayOfWeek))
                    {
                        Shiftdetail shiftdetail = new Shiftdetail();
                        shiftdetail.Shiftid = shift.Shiftid;
                        shiftdetail.Shiftdate = currentDate;
                        shiftdetail.Starttime = StartTimewithdate;
                        shiftdetail.Endtime = EndTimewithdate;
                        shiftdetail.Status = 1;
                        shiftdetail.Regionid = model.region;
                        BitArray forfalse = new BitArray(1);
                        forfalse[0] = false;
                        shiftdetail.Isdeleted = forfalse;
                        _db.Shiftdetails.Add(shiftdetail);
                        _db.SaveChanges();
                        Shiftdetailregion shiftdetailregion = new Shiftdetailregion();
                        shiftdetailregion.Shiftdetailid = shiftdetail.Shiftdetailid;
                        shiftdetailregion.Regionid = model.region;
                        shiftdetailregion.Isdeleted = forfalse;
                        _db.Shiftdetailregions.Add(shiftdetailregion);
                        _db.SaveChanges();
                    }
                }
                currentDate = currentDate.AddDays(1);
            }
            if (shownmsg)
            {
                TempData["Message"] = "Shift Created for : " + _physician.GetFirstOrDefault(x => x.Physicianid == shift.Physicianid).Firstname;
            }
            else
            {
                TempData["Message"] = "Some Shifts  can not be created due to overlaping of time ";
            }
            var datetoshow = new DateTime(shift.Startdate.Year, shift.Startdate.Month, shift.Startdate.Day);
            return RedirectToAction(model.format, new { datetoshow });
        }



        /// <summary>
        /// Scheduling
        /// </summary>
        /// <returns></returns>


        public string regioncolormap(int regionid)
        {
            switch (regionid)
            {
                case 3:
                    return "#3090C7";
                case 4:
                    return "#C2B280";
                case 5:
                    return "#99c68e";
                case 6:
                    return "#98AFC7";
                default:
                    return "blue";
            }
        }
        [Area("AdminArea")]
        public IActionResult Scheduling()
        {
            return View();
        }

        [Area("AdminArea")]
        public IActionResult CreateShiftPopUp(string format)
        {
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var role = "";
            if (roleClaim != null)
            {
                role = roleClaim.Value;
            }
            if (_physician.GetSessionPhysicianId() != -1)
            {
                var phyid = _physician.GetSessionPhysicianId();
                var phyname = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId()).Firstname + " " +
                    _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId()).Lastname;
                return PartialView("_CreateShiftPopUp", new { format = format, role = role, phyid = phyid, phyname = phyname });
            }
            return PartialView("_CreateShiftPopUp", new { format = format, role = role });
        }

        //[Area("AdminArea")]
        //public void SetAllPhysican()
        //{
        //    List<PhysicianData> physicianDatas = new List<PhysicianData>();
        //    var phy = _physician.GetAll().Where(x => x.Isdeleted[0] == false);
        //    foreach (var item in phy)
        //    {
        //        physicianDatas.Add(new PhysicianData { Physicianid = item.Physicianid , Physicianname = item.Firstname + " " + item.Lastname});
        //    }
        //    ViewBag.Physician = physicianDatas;
        //}       

        [Area("AdminArea")]
        public IActionResult DayWiseData(DateTime datetoshow, int region, int status)
        {
            SchedulingDataViewModel schedulingDataViewModel = new SchedulingDataViewModel();
            schedulingDataViewModel.physicianDatas = _scheduling.GetPhysicianData();
            schedulingDataViewModel.Shifts = _scheduling.ShifsOfDate(datetoshow, region, status, 0);
            return PartialView("_Daywisedata", schedulingDataViewModel);
        }

        [Area("AdminArea")]
        [HttpPost]
        public SchedulingDataViewModel DateWiseData(DateTime datetoshow, int region, int status, int next, int pagesize)
        {
            SchedulingDataViewModel schedulingDataViewModel = new SchedulingDataViewModel();


            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var role = "";
            if (roleClaim != null)
            {
                role = roleClaim.Value;
            }
            if (role == "Physician")
            {
                var physicianid = _physician.GetSessionPhysicianId();
                var list = _scheduling.ShifsOfDateOfProvider(datetoshow, status, next, physicianid, pagesize);
                schedulingDataViewModel.Shifts = list;
            }
            if (role == "Admin")
            {
                schedulingDataViewModel.Shifts = _scheduling.ShifsOfDateforMonth(datetoshow, region, status, next, pagesize);
            }
            return schedulingDataViewModel;
        }


        [Area("AdminArea")]
        public IActionResult WeekWiseData(DateTime datetoshow, int region, int status)
        {
            SchedulingDataViewModel schedulingDataViewModel = new SchedulingDataViewModel();
            schedulingDataViewModel.physicianDatas = _scheduling.GetPhysicianData();
            int daysFromSunday = (int)datetoshow.DayOfWeek;
            DateTime sunday = datetoshow.AddDays(-daysFromSunday);
            var shifts = _scheduling.ShifsOfWeek(datetoshow, region, status, 0);
            schedulingDataViewModel.Shifts = shifts;
            List<int> weekdate = new List<int>();
            var firstday = sunday;
            for (int i = 0; i < 7; i++)
            {
                weekdate.Add(firstday.Day);
                firstday = firstday.AddDays(1);
            }
            schedulingDataViewModel.WeekDates = weekdate;
            return PartialView("_Weekwisedata", schedulingDataViewModel);
        }


        [Area("AdminArea")]
        public IActionResult MonthWiseData(DateTime datetoshow, int region, int status)
        {
            SchedulingDataViewModel schedulingDataViewModel = new SchedulingDataViewModel();
            //schedulingDataViewModel.physicianDatas = _scheduling.GetPhysicianData();
            DateTime firstDayOfMonth = new DateTime(datetoshow.Year, datetoshow.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            //var shifts = _scheduling.ShifsOfMonth(datetoshow, region, status, 0);
            //schedulingDataViewModel.Shifts = shifts;
            schedulingDataViewModel.firstMonthdate = DateOnly.FromDateTime(firstDayOfMonth);
            schedulingDataViewModel.lastMonthdate = DateOnly.FromDateTime(lastDayOfMonth);

            return PartialView("_Monthwisedata", schedulingDataViewModel);
        }

        [Area("AdminArea")]
        [HttpPost]
        public List<ShiftData> GetNextShiftsOfDate(DateTime datetoshow, int region, int status, int next)
        {
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var role = "";
            if (roleClaim != null)
            {
                role = roleClaim.Value;
            }
            if (role == "Physician")
            {
                var physicianid = _physician.GetSessionPhysicianId();
                var list = _scheduling.ShifsOfDateOfProvider(datetoshow, status, next, physicianid, 3);
                return list;
            }
            if (role == "Admin")
            {
                var list = _scheduling.ShifsOfDate(datetoshow, region, status, next);
                return list;
            }
            return null;
        }


        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ViewShiftPopUp(int ShiftDetailId, string format)
        {
            var shiftdetail = _db.Shiftdetails.Include(x => x.Shift).Include(x => x.Shift.Physician).Include(x => x.Shiftdetailregions).FirstOrDefault(x => x.Shiftdetailid == ShiftDetailId);
            ShiftData model = new ShiftData();
            model.ShiftId = ShiftDetailId;
            model.Physicianid = shiftdetail.Shift.Physicianid;
            model.Physicianname = shiftdetail.Shift.Physician.Firstname + " " + shiftdetail.Shift.Physician.Lastname;
            model.Location = shiftdetail.Regionid.ToString();
            model.Shiftdate = shiftdetail.Shiftdate;
            model.StartTime = shiftdetail.Starttime;
            model.EndTime = shiftdetail.Endtime;
            model.format = format;
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var role = "";
            if (roleClaim != null)
            {
                role = roleClaim.Value;
            }
            model.loggedrole = role;
            return PartialView("_ViewShiftPopUp", model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult DeleteShift(int shiftdetailid, string format, int status, int region)
        {
            var date = _shiftDetail.GetFirstOrDefault(x => x.Shiftdetailid == shiftdetailid).Shiftdate;
            var datetoshow = new DateTime(date.Year, date.Month, date.Day);
            var modifiedby = "";
            if (_admin.GetSessionAdminId() != -1)
            {
                modifiedby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
            }
            if (_physician.GetSessionPhysicianId() != -1)
            {
                modifiedby = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId()).Aspnetuserid;
            }
            _shiftDetail.DeleteThisShift(shiftdetailid, modifiedby);
            return RedirectToAction(format, new { datetoshow, region, status });
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult RetuenShift(int shiftdetailid, string format, int status, int region)
        {
            var date = _shiftDetail.GetFirstOrDefault(x => x.Shiftdetailid == shiftdetailid).Shiftdate;
            var datetoshow = new DateTime(date.Year, date.Month, date.Day);
            var modifiedby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
            _shiftDetail.ReturnThisShift(shiftdetailid, modifiedby);
            return RedirectToAction(format, new { datetoshow, region, status });
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditShift(int shiftdetailid, string shiftdate, string starttime, string endtime, string format, int status, int region)
        {
            var shiftdetail = _shiftDetail.GetFirstOrDefault(x => x.Shiftdetailid == shiftdetailid);
            var date = shiftdetail.Shiftdate;
            var datetoshow = new DateTime(date.Year, date.Month, date.Day);
            var currentDate = DateOnly.Parse(shiftdate);
            var startTimeParts = starttime.Split(':');
            var endTimeParts = endtime.Split(':');
            var startTime = new TimeSpan(int.Parse(startTimeParts[0]), int.Parse(startTimeParts[1]), 0);
            var endTime = new TimeSpan(int.Parse(endTimeParts[0]), int.Parse(endTimeParts[1]), 0);
            var StartTimewithdate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startTime.Hours, startTime.Minutes, 0);
            var EndTimewithdate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, endTime.Hours, endTime.Minutes, 0);
            var modifiedby = "";
            if (_admin.GetSessionAdminId() != -1)
            {
                modifiedby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
            }
            if (_physician.GetSessionPhysicianId() != -1)
            {
                modifiedby = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId()).Aspnetuserid;
            }
            var edited = _shiftDetail.EditShiftDetail(shiftdetailid, currentDate, StartTimewithdate, EndTimewithdate, modifiedby);
            if (edited)
            {
                return RedirectToAction(format, new { datetoshow, region, status });
            }
            return BadRequest("There is already a shift exist");
        }

        /// <summary>
        /// provideroncall
        /// </summary>
        /// <returns></returns>

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ProviderOnCall(DateTime datetoshow, int region, string showby)
        {
            ProviderOnCallViewModel model = new ProviderOnCallViewModel();
            List<PhysicianData> physicianDatas = new List<PhysicianData>();
            List<int> OnCallIds = new List<int>();
            List<int> OffDutyIds = new List<int>();
            //if (showby == "DayWiseData")
            //{

            DateTime dateTime = DateTime.Now;

            var shifts = _scheduling.ShifsOfDate(dateTime, region, 0, 0).Where(x => (x.StartTime <= dateTime && x.EndTime >= dateTime)).Where(x=>x.Status == 2).Select(x => x.ShiftId).ToList();
            foreach (var shift in shifts)
            {
                var shiftid = _shiftDetail.GetFirstOrDefault(x => x.Shiftdetailid == shift).Shiftid;
                var physicianId = _shift.GetFirstOrDefault(x => x.Shiftid == shiftid).Physicianid;
                if (!OnCallIds.Contains(physicianId))
                {
                    OnCallIds.Add(physicianId);
                }
            }

            //}
            //else if (showby == "WeekWiseData")
            //{
            //    var shifts = _scheduling.ShifsOfWeek(datetoshow, region, 0, 0).Select(x => x.ShiftId).ToList();
            //    foreach (var shift in shifts)
            //    {
            //        var shiftid = _shiftDetail.GetFirstOrDefault(x => x.Shiftdetailid == shift).Shiftid;
            //        var physicianId = _shift.GetFirstOrDefault(x => x.Shiftid == shiftid).Physicianid;
            //        if (!OnCallIds.Contains(physicianId))
            //        {
            //            OnCallIds.Add(physicianId);
            //        }
            //    }
            //}
            //else
            //{
            //    var shifts = _scheduling.ShifsOfMonth(datetoshow, region, 0, 0).Select(x => x.ShiftId).ToList();
            //    foreach (var shift in shifts)
            //    {
            //        var shiftid = _shiftDetail.GetFirstOrDefault(x => x.Shiftdetailid == shift).Shiftid;
            //        var physicianId = _shift.GetFirstOrDefault(x => x.Shiftid == shiftid).Physicianid;
            //        if (!OnCallIds.Contains(physicianId))
            //        {
            //            OnCallIds.Add(physicianId);
            //        }
            //    }
            //}
            var allphyids = _physician.GetAll().Select(x => x.Physicianid).ToList();
            OffDutyIds = allphyids.Except(OnCallIds).ToList();
            foreach (var phyid in OnCallIds)
            {
                var phy = _physician.GetFirstOrDefault(x => x.Physicianid == phyid);
                PhysicianData physicianData = new PhysicianData
                {
                    Physicianid = phy.Physicianid,
                    Physicianname = phy.Firstname + " " + phy.Lastname,
                    Photo = phy.Photo,
                    OnCall = true
                };
                physicianDatas.Add(physicianData);
            }
            foreach (var phyid in OffDutyIds)
            {
                var phy = _physician.GetFirstOrDefault(x => x.Physicianid == phyid);
                PhysicianData physicianData = new PhysicianData
                {
                    Physicianid = phy.Physicianid,
                    Physicianname = phy.Firstname + " " + phy.Lastname,
                    Photo = phy.Photo,
                    OnCall = false
                };
                physicianDatas.Add(physicianData);
            }
            var filtered = new List<PhysicianData>();
            if (region != 0)
            {
                foreach (var phy in physicianDatas)
                {
                    var regionphy = _db.Physicianregions.Where(x => x.Physicianid == phy.Physicianid).Select(x => x.Regionid);
                    if (regionphy.Contains(region))
                    {
                        filtered.Add(phy);
                    }
                }
            }
            else
            {
                filtered = physicianDatas;
            }
            model.showby = showby;
            model.datetoshow = datetoshow;
            model.region = region;
            model.physicianDatas = filtered;
            return View(model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ShiftsAgenda(DateTime datetoshow, int region, string showby)
        {
            return View(new { datetoshow, region, showby });
        }

        [Area("AdminArea")]
        //[HttpPost]
        public IActionResult ShiftDataForAgenda(DateTime datetoshow, int region, string showby, int currentpage)
        {
            SchedulingDataViewModel model = new SchedulingDataViewModel();
            if (showby == "DayWiseData")
            {
                model.Shifts = _scheduling.ShifsOfDate(datetoshow, region, 1, currentpage);
            }
            else if (showby == "WeekWiseData")
            {
                model.Shifts = _scheduling.ShifsOfWeek(datetoshow, region, 1, currentpage);
            }
            else
            {
                model.Shifts = _scheduling.ShifsOfMonth(datetoshow, region, 1, currentpage);
            }
            List<PhysicianData> physicianDatas = new List<PhysicianData>();
            foreach (var s in model.Shifts)
            {
                var phy = _physician.GetFirstOrDefault(x => x.Physicianid == s.Physicianid);
                PhysicianData physician = new PhysicianData
                {
                    Physicianid = phy.Physicianid,
                    Physicianname = phy.Firstname + " " + phy.Lastname,
                    Photo = phy.Photo,
                    OnCall = true,
                };
                physicianDatas.Add(physician);

            }
            model.physicianDatas = physicianDatas;
            return PartialView("_RequestedShifts", model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public int ShiftCountbyFilter(DateTime datetoshow, int region, string showby, int status)
        {
            SchedulingDataViewModel model = new SchedulingDataViewModel();
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var role = "";
            if (roleClaim != null)
            {
                role = roleClaim.Value;
            }
            if (role == "Physician")
            {
                var physicianid = _physician.GetSessionPhysicianId();
                var list = _scheduling.ShifsOfDateOfProvider(datetoshow, status, 0, physicianid, 0);
                return list.Count;
            }
            if (role == "Admin")
            {
                if (showby == "DayWiseData")
                {
                    model.Shifts = _scheduling.ShifsOfDate(datetoshow, region, status, 0);
                }
                else if (showby == "WeekWiseData")
                {
                    model.Shifts = _scheduling.ShifsOfWeek(datetoshow, region, status, 0);
                }
                else
                {
                    model.Shifts = _scheduling.ShifsOfMonth(datetoshow, region, status, 0);
                }
            }
            return model.Shifts.Count();
        }

        [Area("AdminArea")]
        [HttpPost]
        public void DeleteSelecetdShifts(List<int> shiftdetailsid)
        {
            var modifiedby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
            foreach (var shiift in shiftdetailsid)
            {
                _shiftDetail.DeleteThisShift(shiift, modifiedby);
            }
        }
        [Area("AdminArea")]
        [HttpPost]
        public void ApproveSelecetdShifts(List<int> shiftdetailsid)
        {
            var modifiedby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
            foreach (var shiift in shiftdetailsid)
            {
                _shiftDetail.ReturnThisShift(shiift, modifiedby);
            }
        }

        /// <summary>
        /// Invoicing
        /// </summary>
        /// <returns></returns>


        [Area("AdminArea")]
        [HttpPost]
        public bool CheckEmailForPhysician(int id, string email)
        {
            bool phy = _physician.CheckEmailExist(id, email);
            return phy;
        }

        [Area("AdminArea")]
        public IActionResult Invoicing()
        {
            return View();
        }

    }
}
