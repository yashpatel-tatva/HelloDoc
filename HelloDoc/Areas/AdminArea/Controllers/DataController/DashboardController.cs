using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc.Areas.PatientArea.ViewModels;
using HelloDoc;
using HelloDoc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Net;
using DataAccess.ServiceRepository;
using System.Globalization;
using System.Drawing;

namespace HelloDoc.Areas.AdminArea.DataController
{
    [AuthorizationRepository("Admin")]
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
            IAllRequestDataRepository allRequestDataRepository1
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
        }

        // data from tables start

        [Area("AdminArea")]
        public List<Casetag> GetCaseTags()
        {
            var casetags = _db.Casetags.ToList();
            return casetags;
        }

        [Area("AdminArea")]
        public List<Region> GetRegion()
        {
            var regions = _db.Regions.ToList();
            return regions;
        }

        [Area("AdminArea")]
        public List<Physician> GetPhysician()
        {
            var physician = _db.Physicians.ToList();
            return physician;
        }

        [Area("AdminArea")]
        [HttpPost]
        public List<Physician> GetPhysician(int regionid)
        {
            var physician = _db.Physicians.ToList();
            if (regionid != 0)
            {
                physician = physician.Where(x => x.Regionid == regionid).ToList();
            }
            return physician;
        }

        [Area("AdminArea")]
        public List<Healthprofessionaltype> GetProfession()
        {
            var healthproffession = _db.Healthprofessionaltypes.ToList();
            return healthproffession;
        }
        [Area("AdminArea")]
        public List<Healthprofessional> GetVendorbyProfession(int professoinid)
        {
            var healthproffession = _db.Healthprofessionals.ToList().Where(x => x.Profession == professoinid).ToList();
            return healthproffession;
        }
        [Area("AdminArea")]
        public Healthprofessional GetVendorbyVendorid(int vendorid)
        {
            var healthproffession = _db.Healthprofessionals.FirstOrDefault(x => x.Vendorid == vendorid);
            return healthproffession;
        }


        // data from tables end



        [Area("AdminArea")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Area("AdminArea")]

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
        [HttpGet]
        public IActionResult ExportAll()
        {
            List<Request> model1 = _requests.GetAll().ToList();
            List<AllRequestDataViewModel> filtereddata = _allrequestdata.FilteredRequest(model1);
            var record = _allrequest.DownloadExcle(filtereddata);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"All Request_{strDate}.xlsx";

            return File(record, contentType, filename);
        }

        [Area("AdminArea")]
        public IActionResult CreateRequest(FamilyRequestViewModel model)
        {
            var admin = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId());
            model.F_FirstName = admin.Firstname;
            model.F_LastName = admin.Lastname;
            model.F_Email = admin.Email;
            model.F_Phone = admin.Mobile;
            return View(model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult CreateRequestsubmit(FamilyRequestViewModel model)
        {
            _allrequest.AddRequestasAdmin(model);
            return RedirectToAction("AdminTabsLayout", "Home");
        }



        // ViewCase start

        [Area("AdminArea")]
        [HttpGet]
        public IActionResult ViewCase(int id)
        {
            var result = _allrequest.GetRequestById(id);
            return View(result);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditEmailPhone([FromBody] RequestDataViewModel model)
        {
            _allrequest.EditEmailPhone(model);
            return RedirectToAction(model.pageredirectto, "Dashboard", new { id = model.RequestId });
        }

        //Viewcase end

        //ViewNotes start

        [Area("AdminArea")]
        [HttpGet]
        public IActionResult ViewNotes(int id)
        {
            var result = _allrequest.GetNotesById(id);
            return View(result);
        }

        [Area("AdminArea")]
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
            _requestpopupaction.CancelCase(requestid, casetag, note);
            return RedirectToAction("AdminTabsLayout", "Home");

        }

        [Area("AdminArea")]
        [HttpGet]
        public IActionResult BlockCase(int id)
        {
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();
            dashpopupsViewModel.RequestId = id;
            dashpopupsViewModel.PatientName = _requests.GetFirstOrDefault(x => x.Requestid == id).Firstname + " " + _requests.GetFirstOrDefault(x => x.Requestid == id).Lastname;
            return PartialView("_BlockCasePopUp", dashpopupsViewModel);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult BlockCase(int requestid, string note)
        {
            _requestpopupaction.BlockCase(requestid, note);
            return RedirectToAction("AdminTabsLayout", "Home");
        }


        [Area("AdminArea")]
        [HttpGet]
        public IActionResult AssignCase(int id)
        {
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();
            dashpopupsViewModel.RequestId = id;
            dashpopupsViewModel.regions = _db.Regions.ToList();
            dashpopupsViewModel.physicians = _db.Physicians.ToList();
            return PartialView("_AssignCasePopUp", dashpopupsViewModel);
        }
        [Area("AdminArea")]
        [HttpGet]
        public IActionResult TransferCase(int id)
        {
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();
            dashpopupsViewModel.RequestId = id;
            dashpopupsViewModel.regions = _db.Regions.ToList();
            dashpopupsViewModel.physicians = _db.Physicians.ToList();
            return PartialView("_TransferCasePopUp", dashpopupsViewModel);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult AssignCase(int requestid, int phyid, string note)
        {
            _requestpopupaction.AssignCase(requestid, phyid, _admin.GetSessionAdminId(), note);
            return RedirectToAction("AdminTabsLayout", "Home");
        }



        [Area("AdminArea")]
        [HttpGet]
        public IActionResult ClearCase(int id)
        {
            DashpopupsViewModel dashpopupsViewModel = new DashpopupsViewModel();
            dashpopupsViewModel.RequestId = id;
            return PartialView("_ClearCasePopUp", dashpopupsViewModel);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult ClearCaseSubmit(int requestid)
        {
            _requestpopupaction.ClearCase(requestid);
            return RedirectToAction("AdminTabsLayout", "Home");
        }

        [Area("AdminArea")]
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
        [HttpPost]
        public IActionResult SendAgreenment(int requestid, string mobile, string email)
        {
            var requestidcipher = EncryptionRepository.Encrypt(requestid.ToString());
            //var reqplai = EncryptionRepository.Decrypt(requestidcipher);
            var link = "https://localhost:7249/PatientArea/Home/ViewAgreement?requestid=" + requestidcipher;
            _sendemail.Sendemail(email, "View Agreenment", link);
            return RedirectToAction("AdminTabsLayout", "Home");
        }

        [Area("AdminArea")]
        [HttpPost]
        public void PatientAgree(int requestid)
        {
            var request = _requests.GetFirstOrDefault(x => x.Requestid == requestid);
            request.Status = 4;
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
        }

        [Area("AdminArea")]
        public IActionResult RequestDTYSupport()
        {
            return PartialView("_RequestSupport");
        }

        [Area("AdminArea")]
        public IActionResult Sendlinktorequest()
        {
            return PartialView("_SendLinkPopUp");
        }

        [Area("AdminArea")]
        [HttpPost]
        public void SendEmailFromSendLinkPopUp(string firstname , string lastname , string email ,string mobile)
        {
            var url = Url.Action("EmaillinkToOpenPatientRequest", "RequestForms", new { Area = "PatientArea", firstname = firstname , lastname = lastname , email = email , mobile = mobile }, Request.Scheme, Request.Host.Value);
            _sendemail.Sendemail(email, "Submit Your Request" , url);
        }
        //Pop-up ends

        // View uploads start

        [Area("AdminArea")]
        [HttpGet]
        public IActionResult ViewUploads(int id)
        {
            var result = _allrequest.GetDocumentByRequestId(id);
            return View(result);
        }

        [Area("AdminArea")]
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
        [HttpPost]
        public string ViewFile(int id)
        {
            var path = (_db.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == id)).Filename;
            var filename = Path.GetFileName(path);
            return filename;
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _documents.DeleteFile(id);
            var requestid = _requestwisefile.GetFirstOrDefault(x => x.Requestwisefileid == id).Requestid;
            return RedirectToAction("ViewUploads", "Dashboard", new { id = requestid });
        }
        [Area("AdminArea")]
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
        [HttpPost]
        public IActionResult SendMail(List<int> RequestWiseFileId, int RequestsId)
        {
            List<string> filenames = new List<string>();
            foreach (var s in RequestWiseFileId)
            {
                var file = _db.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == s).Filename;
                filenames.Add(file);
            }

            _sendemail.SendEmailwithAttachments("vinit2273@gmail.com", "Your Attachments", "Please Find Your Attachments Here", filenames);
            return RedirectToAction("ViewUploads", "Dashboard", new { id = RequestsId });
        }



        [Area("AdminArea")]
        [HttpPost]
        public IActionResult UploadFiles(List<IFormFile> files, int RequestsId)
        {
            _requestwisefile.Add(RequestsId, files);
            return RedirectToAction("ViewUploads", "Dashboard", new { id = RequestsId });
        }

        // View uploads end




        // orders start

        [Area("AdminArea")]
        [HttpGet]
        public IActionResult Orders(int id)
        {
            SendOrderViewModel model = new SendOrderViewModel();
            model.RequestId = id;
            return View(model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult SendOrderDetails(SendOrderViewModel model)
        {
            model.CreatedBy = HttpContext.Session.GetString("AspNetId");
            _orderDetail.Add(model);
            return RedirectToAction("AdminTabsLayout", "Home");
        }



        //order end


        // Close Case Start

        [Area("AdminArea")]
        [HttpGet]
        public IActionResult CloseCase(int id)
        {
            RequestViewUploadsViewModel model = new RequestViewUploadsViewModel();
            model = _allrequest.GetDocumentByRequestId(id);
            return View(model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult CloseCaseSubmit(int requestid)
        {
            _requestpopupaction.CloseCase(requestid);
            return RedirectToAction("AdminTabsLayout", "Home");
        }

        // Close Case End

    }
}
