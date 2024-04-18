using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using DataModels.CommonViewModel;
using HelloDoc.Areas.PatientArea.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;

namespace HelloDoc.Areas.AdminArea.DataController
{
    public class DashboardController : Controller
    {
        private readonly HelloDocDbContext _db;
        public readonly IAdminRepository _admin;
        public readonly IRequestRepository _requests;
        private readonly IAllRequestDataRepository _allrequest;
        private readonly IBlockCaseRepository _blockcase;
        private readonly IRequestPopUpActionsRepository _requestpopupaction;
        private readonly IDocumentsRepository _documents;
        private readonly IRequestwisefileRepository _requestwisefile;
        private readonly ISendEmailRepository _sendemail;
        private readonly IOrderDetailRepository _orderDetail;
        private readonly IPaginationRepository _paginator;
        private readonly IAllRequestDataRepository _allrequestdata;
        private readonly IPhysicianRepository _physician;
        private readonly IShiftDetailRepository _shiftDetail;
        private readonly IShiftRepository _shift;
        private readonly ISchedulingRepository _scheduling;

        public DashboardController(
            HelloDocDbContext db,
            IAdminRepository adminRepository,
            IRequestRepository requestRepository,
            IAllRequestDataRepository allRequestDataRepository,
            IBlockCaseRepository blockCaseRepository,
            IRequestPopUpActionsRepository requestPopUpActionsRepository,
            IDocumentsRepository documentsRepository,
            IRequestwisefileRepository requestwisefileRepository,
            ISendEmailRepository sendEmailRepository,
            IOrderDetailRepository orderDetailRepository,
            IPaginationRepository paginationRepository,
            IAllRequestDataRepository allRequestDataRepository1,
            IPhysicianRepository physicianRepository,
            IShiftDetailRepository shiftDetailRepository,
            IShiftRepository shiftRepository,
            ISchedulingRepository schedulingRepository
            )
        {
            _db = db;
            _admin = adminRepository;
            _requests = requestRepository;
            _allrequest = allRequestDataRepository;
            _blockcase = blockCaseRepository;
            _requestpopupaction = requestPopUpActionsRepository;
            _documents = documentsRepository;
            _requestwisefile = requestwisefileRepository;
            _sendemail = sendEmailRepository;
            _orderDetail = orderDetailRepository;
            _paginator = paginationRepository;
            _allrequestdata = allRequestDataRepository1;
            _physician = physicianRepository;
            _shift = shiftRepository;
            _scheduling = schedulingRepository;
            _shiftDetail = shiftDetailRepository;
        }

        // data from tables start

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        public List<Casetag> GetCaseTags()
        {
            var casetags = _db.Casetags.ToList();
            return casetags;
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        public List<RegionDataModel> GetRegion()
        {
            var region = _db.Regions.ToList();
            List<RegionDataModel> regions = new List<RegionDataModel>();
            foreach (var r in region)
            {
                regions.Add(new RegionDataModel
                {
                    Name = r.Name,
                    Regionid = r.Regionid,
                });
            }
            return regions;
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        public List<Region> GetRegionofPhysician()
        {
            if (_physician.GetSessionPhysicianId() != -1)
            {
                var regions = _db.Physicianregions.Include(x => x.Region).Where(x => x.Physicianid == _physician.GetSessionPhysicianId()).Select(x => x.Region).ToList();
                return regions;
            }

            return null;
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        public List<Physician> GetPhysician(int requestid)
        {
            var request = _requests.GetFirstOrDefault(x => x.Requestid == requestid);
            var physician = _db.Physicians.Where(x => x.Physicianid != request.Physicianid).ToList();
            physician = physician.Where(x => x.Isdeleted[0] == false).ToList();
            return physician;
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        public List<Aspnetrole> GetRoles()
        {
            return _db.Aspnetroles.ToList();
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public List<Physician> GetPhysician(int regionid, int requestid)
        {
            var request = _requests.GetFirstOrDefault(x => x.Requestid == requestid);
            var physician = _db.Physicians.Include(x => x.Physicianregions).Where(x => x.Physicianid != request.Physicianid).ToList();
            var phyregion = _db.Physicianregions.Include(x => x.Physician).ToList();
            if (regionid != 0)
            {
                physician = phyregion.Where(x => x.Regionid == regionid).Select(x => x.Physician).Where(x => x.Physicianid != request.Physicianid).ToList();
            }
            physician = physician.Where(x => x.Isdeleted[0] == false).ToList();
            List<Physician> result = new List<Physician>();
            foreach (var phy in physician)
            {
                Physician model = new Physician();
                model.Firstname = phy.Firstname;
                model.Lastname = phy.Lastname;
                model.Physicianid = phy.Physicianid;
                result.Add(model);
            }
            return result;
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public List<Physician> GetPhysicianByRegion(int regionid)
        {
            var physician = _db.Physicians.Include(x => x.Physicianregions).ToList();
            var phyregion = _db.Physicianregions.Include(x => x.Physician).ToList();
            if (regionid != 0)
            {
                physician = phyregion.Where(x => x.Regionid == regionid).Select(x => x.Physician).ToList();
            }
            List<Physician> result = new List<Physician>();
            foreach (var phy in physician)
            {
                Physician model = new Physician();
                model.Firstname = phy.Firstname;
                model.Lastname = phy.Lastname;
                model.Physicianid = phy.Physicianid;
                result.Add(model);
            }
            return result;
        }
        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        public List<Healthprofessionaltype> GetProfession()
        {
            var healthproffession = _db.Healthprofessionaltypes.ToList();
            return healthproffession;
        }
        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        public List<Healthprofessional> GetVendorbyProfession(int professoinid)
        {
            var healthproffession = _db.Healthprofessionals.ToList().Where(x => x.Profession == professoinid).ToList();
            return healthproffession;
        }
        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        public Healthprofessional GetVendorbyVendorid(int vendorid)
        {
            var healthproffession = _db.Healthprofessionals.FirstOrDefault(x => x.Vendorid == vendorid);
            return healthproffession;
        }


        // data from tables end



        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]

        public IActionResult Home()
        {
            AdminDashboardViewModel model = new AdminDashboardViewModel();
            model.admin = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId());
            model.newrequest = _requests.Countbystate("New");
            model.pendingrequest = _requests.Countbystate("Pending");
            model.activerequest = _requests.Countbystate("Active");
            model.concluderequest = _requests.Countbystate("Conclude");
            model.tocloserequest = _requests.Countbystate("Toclose");
            model.unpaidrequest = _requests.Countbystate("Unpaid");
            return View(model);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpGet]
        public IActionResult Export(string state, int requesttype, string search, int region)
        {
            List<Request> model1 = _paginator.requests(state, 0, 0, requesttype, search, region);
            List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
            var record = _allrequest.DownloadExcle(filtereddata);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"{state}_{strDate}.xlsx";

            return File(record, contentType, filename);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpGet]
        public IActionResult ExportAll()
        {
            List<Request> model1 = _db.Requests.Include(r => r.User).Include(r => r.Requestclients).Include(r => r.Physician).Include(r => r.User.Region).Include(r => r.Requeststatuslogs).ToList();
            List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
            var record = _allrequest.DownloadExcle(filtereddata);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"All Request_{strDate}.xlsx";

            return File(record, contentType, filename);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        public IActionResult CreateRequest(FamilyRequestViewModel model)
        {
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

            if (roleClaim != null)
            {
                var role = roleClaim.Value;
                if (role == "Admin")
                {
                    var admin = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId());
                    model.F_FirstName = admin.Firstname;
                    model.F_LastName = admin.Lastname;
                    model.F_Email = admin.Email;
                    model.F_Phone = admin.Mobile;
                }
                if (role == "Physician")
                {
                    var physician = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId());
                    model.F_FirstName = physician.Firstname;
                    model.F_LastName = physician.Lastname;
                    model.F_Email = physician.Email;
                    model.F_Phone = physician.Mobile;
                }
            }
            return View(model);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult CreateRequestsubmit(FamilyRequestViewModel model)
        {
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

            if (roleClaim != null)
            {
                var role = roleClaim.Value;
                if (role == "Admin")
                {
                    _allrequest.AddRequestasAdmin(model);
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                if(role == "Physician")
                {
                    _allrequest.AddRequestasPhysician(model);
                    return RedirectToAction("Dashboard", "Dashboard" , new {area = "ProviderArea"});
                }
            }
            return BadRequest();
        }



        // ViewCase start

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpGet]
        public IActionResult ViewCase(int id)
        {
            RequestDataViewModel result = _allrequest.GetRequestById(id);
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

            if (roleClaim != null)
            {
                var role = roleClaim.Value;
                result.role = role;
            }
            return View(result);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public IActionResult EditEmailPhone([FromBody] RequestDataViewModel model)
        {
            _allrequest.EditEmailPhone(model);
            return RedirectToAction(model.pageredirectto, "Dashboard", new { id = model.RequestId });
        }

        //Viewcase end

        //ViewNotes start

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpGet]
        public IActionResult ViewNotes(int id)
        {
            RequestNotesViewModel result = _allrequest.GetNotesById(id);
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

            if (roleClaim != null)
            {
                var role = roleClaim.Value;
                result.role = role;
            }
            return View(result);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public IActionResult SaveAdminNotes([FromBody] RequestNotesViewModel model)
        {
            var id = model.RequestId;
            _allrequest.SaveAdminNotes(id, model);
            return RedirectToAction("ViewNotes", "Dashboard", new { id = id });
        }
        // View notes end


        // Pop-ups start

        [Area("AdminArea")]
        [HttpGet]
        public IActionResult CancelCase(int id)
        {
            if (_requests.GetById(id).Status == 4)
            {
                return BadRequest("Your case is now Active. you can not change . please contact provider");
            }
            if (_requests.GetById(id).Status == 3)
            {
                return BadRequest("This case has been camcel before ");
            }
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();
            dashpopupsViewModel.RequestId = id;
            dashpopupsViewModel.PatientName = _requests.GetFirstOrDefault(x => x.Requestid == id).Firstname + " " + _requests.GetFirstOrDefault(x => x.Requestid == id).Lastname;
            dashpopupsViewModel.casetags = _db.Casetags.ToList();
            return PartialView("_CancelCasePopUp", dashpopupsViewModel);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult CancelCase(int requestid, int casetag, string note)
        {
            if (_requests.GetById(requestid).Status == 4)
            {
                return BadRequest("Your case is now Active. you can not change . please contact provider");
            }
            if (_requests.GetById(requestid).Status == 3)
            {
                return BadRequest("This case has been camcel before ");
            }

            _requestpopupaction.CancelCase(requestid, casetag, note);
            return RedirectToAction("Dashboard", "Dashboard");

        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpGet]
        public IActionResult BlockCase(int id)
        {
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();
            dashpopupsViewModel.RequestId = id;
            dashpopupsViewModel.PatientName = _requests.GetFirstOrDefault(x => x.Requestid == id).Firstname + " " + _requests.GetFirstOrDefault(x => x.Requestid == id).Lastname;
            return PartialView("_BlockCasePopUp", dashpopupsViewModel);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public IActionResult BlockCase(int requestid, string note)
        {
            _requestpopupaction.BlockCase(requestid, note);
            return RedirectToAction("Dashboard", "Dashboard");
        }


        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpGet]
        public IActionResult AssignCase(int id)
        {
            var request = _requests.GetFirstOrDefault(x => x.Requestid == id);
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();
            dashpopupsViewModel.RequestId = id;
            dashpopupsViewModel.regions = _db.Regions.ToList();
            dashpopupsViewModel.physicians = _db.Physicians.Where(x => x.Physicianid != request.Physicianid).ToList();
            dashpopupsViewModel.physicians = dashpopupsViewModel.physicians.Where(x => x.Isdeleted[0] == false).ToList();
            return PartialView("_AssignCasePopUp", dashpopupsViewModel);
        }
        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpGet]
        public IActionResult TransferCase(int id)
        {
            var request = _requests.GetFirstOrDefault(x => x.Requestid == id);
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();
            dashpopupsViewModel.RequestId = id;
            dashpopupsViewModel.regions = _db.Regions.ToList();
            dashpopupsViewModel.physicians = _db.Physicians.Where(x => x.Physicianid != request.Physicianid).ToList();
            dashpopupsViewModel.physicians = dashpopupsViewModel.physicians.Where(x => x.Isdeleted[0]==false).ToList();
            return PartialView("_TransferCasePopUp", dashpopupsViewModel);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public IActionResult AssignCase(int requestid, int phyid, string note)
        {
            _requestpopupaction.AssignCase(requestid, phyid, _admin.GetSessionAdminId(), note);
            return RedirectToAction("Dashboard", "Dashboard");
        }



        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpGet]
        public IActionResult ClearCase(int id)
        {
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();
            dashpopupsViewModel.RequestId = id;
            return PartialView("_ClearCasePopUp", dashpopupsViewModel);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public IActionResult ClearCaseSubmit(int requestid)
        {
            _requestpopupaction.ClearCase(requestid);
            return RedirectToAction("Dashboard", "Dashboard");
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpGet]
        public IActionResult SendAgreenment(int id)
        {
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();

            var request = _db.Requests.Include(r => r.Requestclients).FirstOrDefault(r => r.Requestid == id);
            dashpopupsViewModel.RequestId = request.Requestid;
            dashpopupsViewModel.RequestType = request.Requesttypeid;
            dashpopupsViewModel.PatientEmail = request.Requestclients.ElementAt(0).Email;
            dashpopupsViewModel.PatientMobile = request.Requestclients.ElementAt(0).Phonenumber;
            return PartialView("_SendAgreementPopUp", dashpopupsViewModel);
        }


        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult SendAgreenment(int requestid, string mobile, string email)
        {
            var requestidcipher = EncryptionRepository.Encrypt(requestid.ToString());
            //var reqplai = EncryptionRepository.Decrypt(requestidcipher);
            var link = "https://localhost:7249/PatientArea/Home/ViewAgreement?requestid=" + requestidcipher;
            _sendemail.Sendemail(email, "View Agreenment", link);
            return RedirectToAction("Dashboard", "Dashboard");
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult PatientAgree(int requestid)
        {
            var request = _requests.GetFirstOrDefault(x => x.Requestid == requestid);
            if (request.Status == 3)
            {
                return BadRequest("You can't change. Please Contact CustomerCare Service");
            }
            if (request.Status == 4)
            {
                return BadRequest("This is already an active case");
            }
            request.Status = 4;
            request.Accepteddate = DateTime.Now;
            _requests.Update(request);
            _requests.Save();
            var reqstatus = new Requeststatuslog
            {
                Requestid = requestid,
                Status = 4,
                Createddate = DateTime.Now,
            };
            _db.Requeststatuslogs.Add(reqstatus);
            _db.SaveChanges();
            return Ok();
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        public IActionResult RequestDTYSupport()
        {
            return PartialView("_RequestSupport");
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        public IActionResult Sendlinktorequest()
        {
            return PartialView("_SendLinkPopUp");
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public void SendEmailFromSendLinkPopUp(string firstname, string lastname, string email, string mobile)
        {
            var url = Url.Action("EmaillinkToOpenPatientRequest", "RequestForms", new { Area = "PatientArea", firstname = firstname, lastname = lastname, email = email, mobile = mobile }, Request.Scheme, Request.Host.Value);
            _sendemail.Sendemail(email, "Submit Your Request", url);
        }
        //Pop-up ends

        // View uploads start

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpGet]
        public IActionResult ViewUploads(int id)
        {
            RequestViewUploadsViewModel result = _allrequest.GetDocumentByRequestId(id);
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");

            if (roleClaim != null)
            {
                var role = roleClaim.Value;
                result.role = role;
            }
            return View(result);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        public async Task<IActionResult> Download(int id)
        {
            var path = (await _db.Requestwisefiles.FirstOrDefaultAsync(x => x.Requestwisefileid == id)).Filename;
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = _documents.Download(id);
            return File(bytes, contentType, Path.GetFileName(path));
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public string ViewFile(int id)
        {
            var path = (_db.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == id)).Filename;
            var filename = Path.GetFileName(path);
            return filename;
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _documents.DeleteFile(id);
            var requestid = _requestwisefile.GetFirstOrDefault(x => x.Requestwisefileid == id).Requestid;
            return RedirectToAction("ViewUploads", "Dashboard", new { id = requestid });
        }
        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public async Task<IActionResult> Download(PatientDashboardViewModel dashedit)
        {
            var checkbox = Request.Form["downloadselect"].ToList();
            var zipname = dashedit.RequestsId.ToString() + "_" + DateTime.Now + ".zip";
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var item in checkbox)
                    {
                        var s = Int32.Parse(item);
                        var file = await _db.Requestwisefiles.FirstOrDefaultAsync(x => x.Requestwisefileid == s);
                        var path = file.Filename;
                        var bytes = await System.IO.File.ReadAllBytesAsync(path);
                        var zipEntry = zipArchive.CreateEntry(Path.GetFileName(path), CompressionLevel.Fastest);
                        using (var zipStream = zipEntry.Open())
                        {
                            await zipStream.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                }
                memoryStream.Position = 0; // Reset the position
                return File(memoryStream.ToArray(), "application/zip", zipname, enableRangeProcessing: true);
            }
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public IActionResult SendMail(List<int> RequestWiseFileId, int RequestsId)
        {
            var request = _requests.GetFirstOrDefault(c => c.Requestid == RequestsId);
            var emailto = _db.Requestclients.FirstOrDefault(x => x.Requestid == request.Requestid);
            Emaillog emaillog = new Emaillog();
            emaillog.Emailid = emailto.Email;
            emaillog.Filepath = " ";
            List<string> filenames = new List<string>();
            foreach (var s in RequestWiseFileId)
            {
                var file = _db.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == s).Filename;
                filenames.Add(file);
                emaillog.Filepath = emaillog.Filepath + " |||| " + file;
            }
            emaillog.Sentdate = DateTime.Now;
            emaillog.Createdate = DateTime.Now;
            emaillog.Subjectname = "Please Find Your Attachments Here";
            emaillog.Requestid = request.Requestid;
            emaillog.Confirmationnumber = request.Confirmationnumber;
            if (_admin.GetSessionAdminId() != -1)
            {
                emaillog.Roleid = 1;
                emaillog.Adminid = _admin.GetSessionAdminId();
            }
            emaillog.Emailtemplate = "For Files";
            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            emaillog.Isemailsent = fortrue;
            emaillog.Senttries = 1;
            _db.Emaillogs.Add(emaillog);
            _db.SaveChanges();
            _sendemail.SendEmailwithAttachments(emailto.Email, "Your Attachments", "Please Find Your Attachments Here", filenames);
            return RedirectToAction("ViewUploads", "Dashboard", new { id = RequestsId });
        }



        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult UploadFilesfromConclude(List<IFormFile> files, int RequestsId)
        {
            _requestwisefile.Add(RequestsId, files);
            return RedirectToAction("ConcludeCare", "Dashboard", new { area = "ProviderArea", id = RequestsId });
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult UploadFiles(List<IFormFile> files, int RequestsId)
        {
            _requestwisefile.Add(RequestsId, files);
            return RedirectToAction("ViewUploads", "Dashboard", new { id = RequestsId });
        }

        // View uploads end




        // orders start

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpGet]
        public IActionResult Orders(int id)
        {
            SendOrderViewModel model = new SendOrderViewModel();
            model.RequestId = id;
            return View(model);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult SendOrderDetails(SendOrderViewModel model)
        {
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            model.CreatedBy = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId").Value;
            _orderDetail.Add(model);
            TempData["Message"] = "Order Submitted";

            if (roleClaim != null)
            {
                var role = roleClaim.Value;
                if (role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                if (role == "Physician")
                {
                    return RedirectToAction("Dashboard", "Dashboard", new { area = "ProviderArea" });
                }
            }
            return BadRequest();
        }



        //order end


        // Close Case Start

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpGet]
        public IActionResult CloseCase(int id)
        {
            RequestViewUploadsViewModel model = new RequestViewUploadsViewModel();
            model = _allrequest.GetDocumentByRequestId(id);
            return View(model);
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public IActionResult CloseCaseSubmit(int requestid)
        {
            _requestpopupaction.CloseCase(requestid);
            return RedirectToAction("Dashboard", "Dashboard");
        }

        // Close Case End


        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpGet]
        public IActionResult Encounter(int id)
        {
            var request = _requests.GetById(id);
            var encounter = _db.Encounters.FirstOrDefault(x => x.RequestId == id);
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var requestcookie = HttpContext.Request;
            var token = requestcookie.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var role = "";
            if (roleClaim != null)
            {
                role = roleClaim.Value;
            }
            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            BitArray forfalse = new BitArray(1);
            forfalse[0] = false;
            if (request.Status == 4 || (request.Status == 5 && request.Calltype == 1))
            {
                return PartialView("_SelectCallType", request);
            }
            if (request.Status == 6 && encounter == null)
            {
                EncounterFormViewModel model = new EncounterFormViewModel();
                model.Firstname = request.Requestclients.First().Firstname;
                model.Lastname = request.Requestclients.First().Lastname;
                model.DOB = new DateTime(Convert.ToInt32(request.User.Intyear), DateTime.ParseExact(request.User.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(request.User.Intdate)).ToString("yyyy-MM-dd");
                model.Mobile = request.Requestclients.FirstOrDefault().Phonenumber;
                model.Email = request.Requestclients.FirstOrDefault().Email;
                model.Location = request.Requestclients.FirstOrDefault().Address;
                model.isFinaled = !fortrue[0];
                model.RequestId = request.Requestid;
                return View(model);
            }
            else if (request.Status == 6 && encounter.IsFinalized[0] != true)
            {
                EncounterFormViewModel model = new EncounterFormViewModel();
                model.RequestId = request.Requestid;
                model.Firstname = request.Requestclients.First().Firstname;
                model.Lastname = request.Requestclients.First().Lastname;
                model.DOB = new DateTime(Convert.ToInt32(request.User.Intyear), DateTime.ParseExact(request.User.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(request.User.Intdate)).ToString("yyyy-MM-dd");
                model.Mobile = request.Requestclients.FirstOrDefault().Phonenumber;
                model.Email = request.Requestclients.FirstOrDefault().Email;
                model.Location = request.Requestclients.FirstOrDefault().Address;
                model.isFinaled = !fortrue[0];
                model.HistoryOfIllness = encounter.HistoryIllness;
                model.MedicalHistory = encounter.MedicalHistory;
                model.Medication = encounter.Medications;
                model.Allergies = encounter.Allergies;
                model.Temp = encounter.Temp;
                model.HR = encounter.Hr;
                model.RR = encounter.Rr;
                model.BPs = encounter.BpS;
                model.BPd = encounter.BpD;
                model.O2 = encounter.O2;
                model.Pain = encounter.Pain;
                model.Heent = encounter.Heent;
                model.CV = encounter.Cv;
                model.Chest = encounter.Chest;
                model.ABD = encounter.Abd;
                model.Extr = encounter.Extr;
                model.Skin = encounter.Skin;
                model.Neuro = encounter.Neuro;
                model.Other = encounter.Other;
                model.Diagnosis = encounter.Diagnosis;
                model.TreatmentPlan = encounter.TreatmentPlan;
                model.MedicationsDispended = encounter.MedicationDispensed;
                model.Procedure = encounter.Procedures;
                model.Followup = encounter.FollowUp;
                model.role = role;
                return View(model);
            }
            else if ((request.Status == 6 || request.Status == 7 || request.Status == 8 || request.Status == 3) && encounter.IsFinalized != fortrue)
            {
                return PartialView("_DownLoadEncounter", new { requestid = request.Requestid, role = role });
            }
            else
            {
                return PartialView("_SelectCallType", request);
            }
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult EncounterFormSubmit(EncounterFormViewModel model)
        {
            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            var request = _requests.GetById(model.RequestId);
            var encounter = _db.Encounters.FirstOrDefault(x => x.RequestId == request.Requestid);
            if (encounter == null)
            {
                encounter = new Encounter();
            }
            request.Requestclients.First().Firstname = model.Firstname;
            request.Requestclients.First().Lastname = model.Lastname;
            request.Requestclients.FirstOrDefault().Phonenumber = model.Mobile;
            request.Requestclients.FirstOrDefault().Email = model.Email;
            request.Requestclients.FirstOrDefault().Address = model.Location;
            encounter.HistoryIllness = model.HistoryOfIllness;
            encounter.MedicalHistory = model.MedicalHistory;
            encounter.Medications = model.Medication;
            encounter.Allergies = model.Allergies;
            encounter.Temp = model.Temp;
            encounter.Hr = model.HR;
            encounter.Rr = model.RR;
            encounter.BpS = model.BPs;
            encounter.BpD = model.BPd;
            encounter.O2 = model.O2;
            encounter.Pain = model.Pain;
            encounter.Heent = model.Heent;
            encounter.Cv = model.CV;
            encounter.Chest = model.Chest;
            encounter.Abd = model.ABD;
            encounter.Extr = model.Extr;
            encounter.Skin = model.Skin;
            encounter.Neuro = model.Neuro;
            encounter.Other = model.Other;
            encounter.Diagnosis = model.Diagnosis;
            encounter.TreatmentPlan = model.TreatmentPlan;
            encounter.MedicationDispensed = model.MedicationsDispended;
            encounter.Procedures = model.Procedure;
            encounter.FollowUp = model.Followup;
            _db.Requests.Update(request);
            if (encounter.RequestId == 0)
            {
                encounter.RequestId = request.Requestid;
                encounter.Createddate = DateTime.Now;
                encounter.Createdby = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId()).Aspnetuserid;
                _db.Add(encounter);
            }
            else
            {
                _db.Encounters.Update(encounter);
                encounter.Modifieddate = DateTime.Now;
                if (_physician.GetSessionPhysicianId() != -1)
                {
                    encounter.Modifiedby = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId()).Aspnetuserid;
                }
                if (_admin.GetSessionAdminId() != -1)
                {
                    encounter.Modifiedby = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid;
                }
            }
            _db.SaveChanges();
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var requestcookie = HttpContext.Request;
            var token = requestcookie.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var role = "";
            if (roleClaim != null)
            {
                role = roleClaim.Value;
            }
            if (role == "Admin")
            {
                return RedirectToAction("Dashboard", "Dashboard", new { area = "AdminArea" });
            }
            else
            {
                return RedirectToAction("Dashboard", "Dashboard", new { area = "ProviderArea" });
            }
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult CallTypeConsultforRequest(int id, int calltype)
        {
            var requestdetail = _requests.GetById(id);
            if (calltype == 2)
            {
                requestdetail.Status = 6;
                requestdetail.Calltype = (short?)calltype;
                Requeststatuslog requeststatuslog = new Requeststatuslog();
                requeststatuslog.Status = requestdetail.Status;
                requeststatuslog.Requestid = requestdetail.Requestid;
                requeststatuslog.Notes = "Provider choose for consultunt";
                requeststatuslog.Createddate = DateTime.Now;
                requeststatuslog.Physicianid = _physician.GetSessionPhysicianId();
                _db.Requeststatuslogs.Add(requeststatuslog);
                _requests.Update(requestdetail);
                _requests.Save();
            }
            return RedirectToAction("Dashboard", "Dashboard", new { area = "ProviderArea" });
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult FinalizeEncounter(int id)
        {
            BitArray fortrue = new BitArray(1);
            fortrue[0] = true;
            var encounter = _db.Encounters.FirstOrDefault(x => x.RequestId == id);
            if (encounter == null)
            {
                var enounternew = new Encounter();
                enounternew.RequestId = id;
                enounternew.IsFinalized = fortrue;
                enounternew.Date = DateTime.Now;
                enounternew.Createddate = DateTime.Now;
                enounternew.Createdby = _physician.GetFirstOrDefault(x => x.Physicianid == _physician.GetSessionPhysicianId()).Aspnetuserid;
                _db.Encounters.Add(enounternew);
                _db.SaveChanges();
            }
            else
            {
                encounter.IsFinalized = fortrue;
                _db.Encounters.Update(encounter);
                _db.SaveChanges();
            }
            return RedirectToAction("Dashboard", "Dashboard", new { area = "ProviderArea" });
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult OnHouseOpenEncounter(int id)
        {
            var request = _requests.GetById(id);
            request.Status = 6;
            _requests.Update(request);
            Requeststatuslog requeststatuslog = new Requeststatuslog();
            requeststatuslog.Status = request.Status;
            requeststatuslog.Requestid = request.Requestid;
            requeststatuslog.Notes = "Provider choose for housecall";
            requeststatuslog.Createddate = DateTime.Now;
            requeststatuslog.Physicianid = _physician.GetSessionPhysicianId();
            _db.Requeststatuslogs.Add(requeststatuslog);
            _requests.Save();
            return RedirectToAction("Encounter", new { id = id });
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpPost]
        public IActionResult CallTypeHousecallforRequest(int id, int calltype)
        {
            var requestdetail = _requests.GetFirstOrDefault(x => x.Requestid == id);
            if (calltype == 1)
            {
                requestdetail.Status = 5;
                requestdetail.Calltype = (short?)calltype;
                Requeststatuslog requeststatuslog = new Requeststatuslog();
                requeststatuslog.Status = requestdetail.Status;
                requeststatuslog.Requestid = requestdetail.Requestid;
                requeststatuslog.Notes = "Provider chose for consultunt";
                requeststatuslog.Createddate = DateTime.Now;
                requeststatuslog.Physicianid = _physician.GetSessionPhysicianId();
                _db.Requeststatuslogs.Add(requeststatuslog);
                _requests.Update(requestdetail);
                _requests.Save();
            }
            return RedirectToAction("Dashboard", "Dashboard", new { area = "ProviderArea" });
        }

        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public IActionResult EditEncounterAsAdmin(int id)
        {
            var request = _requests.GetById(id);
            var encounter = _db.Encounters.FirstOrDefault(x => x.RequestId == request.Requestid);
            EncounterFormViewModel model = new EncounterFormViewModel();
            model.RequestId = request.Requestid;
            model.Firstname = request.Requestclients.First().Firstname;
            model.Lastname = request.Requestclients.First().Lastname;
            model.DOB = new DateTime(Convert.ToInt32(request.User.Intyear), DateTime.ParseExact(request.User.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(request.User.Intdate)).ToString("yyyy-MM-dd");
            model.Mobile = request.Requestclients.FirstOrDefault().Phonenumber;
            model.Email = request.Requestclients.FirstOrDefault().Email;
            model.Location = request.Requestclients.FirstOrDefault().Address;
            model.HistoryOfIllness = encounter.HistoryIllness;
            model.MedicalHistory = encounter.MedicalHistory;
            model.Medication = encounter.Medications;
            model.Allergies = encounter.Allergies;
            model.Temp = encounter.Temp;
            model.HR = encounter.Hr;
            model.RR = encounter.Rr;
            model.BPs = encounter.BpS;
            model.BPd = encounter.BpD;
            model.O2 = encounter.O2;
            model.Pain = encounter.Pain;
            model.Heent = encounter.Heent;
            model.CV = encounter.Cv;
            model.Chest = encounter.Chest;
            model.ABD = encounter.Abd;
            model.Extr = encounter.Extr;
            model.Skin = encounter.Skin;
            model.Neuro = encounter.Neuro;
            model.Other = encounter.Other;
            model.Diagnosis = encounter.Diagnosis;
            model.TreatmentPlan = encounter.TreatmentPlan;
            model.MedicationsDispended = encounter.MedicationDispensed;
            model.Procedure = encounter.Procedures;
            model.Followup = encounter.FollowUp;
            model.isFinaled = encounter.IsFinalized[0];
            model.role = "Admin";
            return PartialView("Encounter", model);
        }
        [Area("AdminArea")]
        [AuthorizationRepository("Admin,Physician")]
        [HttpGet]
        public IActionResult DownloadEncounterAsAdmin(int id)
        {
            var request = _requests.GetById(id);
            var encounter = _db.Encounters.FirstOrDefault(x => x.RequestId == request.Requestid);
            EncounterFormViewModel model = new EncounterFormViewModel();
            model.RequestId = request.Requestid;
            model.Firstname = request.Requestclients.First().Firstname;
            model.Lastname = request.Requestclients.First().Lastname;
            model.DOB = new DateTime(Convert.ToInt32(request.User.Intyear), DateTime.ParseExact(request.User.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(request.User.Intdate)).ToString("yyyy-MM-dd");
            model.Mobile = request.Requestclients.FirstOrDefault().Phonenumber;
            model.Email = request.Requestclients.FirstOrDefault().Email;
            model.Location = request.Requestclients.FirstOrDefault().Address;
            model.HistoryOfIllness = encounter.HistoryIllness;
            model.MedicalHistory = encounter.MedicalHistory;
            model.Medication = encounter.Medications;
            model.Allergies = encounter.Allergies;
            model.Temp = encounter.Temp;
            model.HR = encounter.Hr;
            model.RR = encounter.Rr;
            model.BPs = encounter.BpS;
            model.BPd = encounter.BpD;
            model.O2 = encounter.O2;
            model.Pain = encounter.Pain;
            model.Heent = encounter.Heent;
            model.CV = encounter.Cv;
            model.Chest = encounter.Chest;
            model.ABD = encounter.Abd;
            model.Extr = encounter.Extr;
            model.Skin = encounter.Skin;
            model.Neuro = encounter.Neuro;
            model.Other = encounter.Other;
            model.Diagnosis = encounter.Diagnosis;
            model.TreatmentPlan = encounter.TreatmentPlan;
            model.MedicationsDispended = encounter.MedicationDispensed;
            model.Procedure = encounter.Procedures;
            model.Followup = encounter.FollowUp;
            model.isFinaled = encounter.IsFinalized[0];
            var pdf = new iTextSharp.text.Document();
            using (var memoryStream = new MemoryStream())
            {
                var writer = PdfWriter.GetInstance(pdf, memoryStream);
                pdf.Open();

                // Create a table with two columns
                PdfPTable table = new PdfPTable(2);


                // Add cells to the table here:
                table.AddCell(CreateCell("First Name"));
                table.AddCell(CreateCell(model.Firstname ?? "N/A"));
                table.AddCell(CreateCell("Last Name"));
                table.AddCell(CreateCell(model.Lastname ?? "N/A"));
                table.AddCell(CreateCell("DOB"));
                table.AddCell(CreateCell(model.DOB?.ToString() ?? "N/A"));
                table.AddCell(CreateCell("Mobile"));
                table.AddCell(CreateCell(model.Mobile ?? "N/A"));
                table.AddCell(CreateCell("Email"));
                table.AddCell(CreateCell(model.Email ?? "N/A"));
                table.AddCell(CreateCell("Location"));
                table.AddCell(CreateCell(model.Location ?? "N/A"));
                table.AddCell(CreateCell("History Of Illness"));
                table.AddCell(CreateCell(model.HistoryOfIllness ?? "N/A"));
                table.AddCell(CreateCell("Medical History"));
                table.AddCell(CreateCell(model.MedicalHistory ?? "N/A"));
                table.AddCell(CreateCell("Medication"));
                table.AddCell(CreateCell(model.Medication ?? "N/A"));
                table.AddCell(CreateCell("Allergies"));
                table.AddCell(CreateCell(model.Allergies ?? "N/A"));
                table.AddCell(CreateCell("Temp"));
                table.AddCell(CreateCell(model.Temp?.ToString() ?? "N/A"));
                table.AddCell(CreateCell("HR"));
                table.AddCell(CreateCell(model.HR?.ToString() ?? "N/A"));
                table.AddCell(CreateCell("RR"));
                table.AddCell(CreateCell(model.RR?.ToString() ?? "N/A"));
                table.AddCell(CreateCell("Blood pressure(Systolic)"));
                table.AddCell(CreateCell(model.BPs?.ToString() ?? "N/A"));
                table.AddCell(CreateCell("Blood pressure(Diastolic)"));
                table.AddCell(CreateCell(model.BPd?.ToString() ?? "N/A"));
                table.AddCell(CreateCell("O2"));
                table.AddCell(CreateCell(model.O2?.ToString() ?? "N/A"));
                table.AddCell(CreateCell("Pain"));
                table.AddCell(CreateCell(model.Pain?.ToString() ?? "N/A"));
                table.AddCell(CreateCell("Heent"));
                table.AddCell(CreateCell(model.Heent ?? "N/A"));
                table.AddCell(CreateCell("CV"));
                table.AddCell(CreateCell(model.CV ?? "N/A"));
                table.AddCell(CreateCell("Chest"));
                table.AddCell(CreateCell(model.Chest ?? "N/A"));
                table.AddCell(CreateCell("ABD"));
                table.AddCell(CreateCell(model.ABD ?? "N/A"));
                table.AddCell(CreateCell("Extr"));
                table.AddCell(CreateCell(model.Extr ?? "N/A"));
                table.AddCell(CreateCell("Skin"));
                table.AddCell(CreateCell(model.Skin ?? "N/A"));
                table.AddCell(CreateCell("Neuro"));
                table.AddCell(CreateCell(model.Neuro ?? "N/A"));
                table.AddCell(CreateCell("Other"));
                table.AddCell(CreateCell(model.Other ?? "N/A"));
                table.AddCell(CreateCell("Diagnosis"));
                table.AddCell(CreateCell(model.Diagnosis ?? "N/A"));
                table.AddCell(CreateCell("Treatment Plan"));
                table.AddCell(CreateCell(model.TreatmentPlan ?? "N/A"));
                table.AddCell(CreateCell("Medications Dispended"));
                table.AddCell(CreateCell(model.MedicationsDispended ?? "N/A"));
                table.AddCell(CreateCell("Procedure"));
                table.AddCell(CreateCell(model.Procedure ?? "N/A"));
                table.AddCell(CreateCell("Followup"));
                table.AddCell(CreateCell(model.Followup ?? "N/A"));
                table.AddCell(CreateCell("Is Finaled"));
                table.AddCell(CreateCell(model.isFinaled.ToString() ?? "N/A"));

                // Add the table to the PDF
                pdf.Add(table);

                pdf.Close();
                writer.Close();

                var bytes = memoryStream.ToArray();
                var result = new FileContentResult(bytes, "application/pdf");
                result.FileDownloadName = "Encounter_" + model.RequestId + ".pdf";
                return result;
            }
        }
        private PdfPCell CreateCell(string content)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content));
            cell.Padding = 5;
            return cell;
        }
        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        [HttpPost]
        public void SendEmailToAllOffduty(string message)
        {
            List<int> OnCallIds = new List<int>();
            List<int> OffDutyIds = new List<int>();

            DateTime dateTime = DateTime.Now;

            var shifts = _scheduling.ShifsOfDate(dateTime, 0, 0, 0).Where(x => (x.StartTime <= dateTime && x.EndTime >= dateTime)).Select(x => x.ShiftId).ToList();
            foreach (var shift in shifts)
            {
                var shiftid = _shiftDetail.GetFirstOrDefault(x => x.Shiftdetailid == shift).Shiftid;
                var physicianId = _shift.GetFirstOrDefault(x => x.Shiftid == shiftid).Physicianid;
                if (!OnCallIds.Contains(physicianId))
                {
                    OnCallIds.Add(physicianId);
                }
            }
            var allphyids = _physician.GetAll().Select(x => x.Physicianid).ToList();
            OffDutyIds = allphyids.Except(OnCallIds).ToList();
            var stopnoti = _db.Physiciannotifications
                              .AsEnumerable()
                              .Where(x => x.Isnotificationstopped[0] == true)
                              .Select(x => x.Pysicianid)
                              .ToList();
            foreach (var id in stopnoti)
            {
                OffDutyIds.Remove(id);
            }
            foreach (var id in OffDutyIds)
            {
                var physicianemail = _physician.GetFirstOrDefault(x => x.Physicianid == id).Email;
                _sendemail.Sendemail(physicianemail, "There is an urgent need to address the shortage of physicians.", message);
            }
        }

    }
}
