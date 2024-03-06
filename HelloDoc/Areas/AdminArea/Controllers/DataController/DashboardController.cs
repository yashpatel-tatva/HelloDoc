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

        public DashboardController(
            HelloDocDbContext db,
            IAdminRepository adminRepository,
            IRequestRepository requestRepository,
            IAllRequestDataRepository allRequestDataRepository,
            IBlockCaseRepository blockCaseRepository,
            IRequestPopUpActionsRepository requestPopUpActionsRepository,
            IDocumentsRepository documentsRepository,
            IRequestwisefileRepository requestwisefileRepository,
            ISendEmailRepository sendEmailRepository
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
        }

        [Area("AdminArea")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Area("AdminArea")]
        public IActionResult Home()
        {
            AdminDashboardViewModel model = new AdminDashboardViewModel();
            if (_admin.GetSessionAdminId() == -1)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            model.admin = _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId());
            model.requests = _requests.GetAll().ToList();
            return View(model);
        }

        [Area("AdminArea")]
        [HttpGet]
        public IActionResult Export(string status)
        {
            var record = _allrequest.DownloadExcle(status);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"{status}_{strDate}.xlsx";

            return File(record, contentType, filename);
        }

        [Area("AdminArea")]
        [HttpGet]
        public IActionResult ExportAll()
        {
            var record = _allrequest.DownloadExcle("all");
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var strDate = DateTime.Now.ToString("yyyyMMdd");
            string filename = $"All Request_{strDate}.xlsx";

            return File(record, contentType, filename);
        }





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
            return RedirectToAction("ViewCase", "Dashboard", new { id = model.RequestId });
        }



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

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult CancelCase(DashpopupsViewModel dashpopupsViewModel)
        {
            _requestpopupaction.CancelCase(dashpopupsViewModel);
            return RedirectToAction("AdminTabsLayout", "Home");

        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult BlockRequest(DashpopupsViewModel model)
        {
            _requestpopupaction.BlockCase(model);
            var result = _blockcase.GetAll();
            return View(result);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult AssignCase(DashpopupsViewModel model)
        {
            _requestpopupaction.AssignCase(model.RequestId, model.PhysicianId, _admin.GetSessionAdminId(), model.Notes);
            return RedirectToAction("AdminTabsLayout", "Home");
        }




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


        //[Area("AdminArea")]
        //[HttpPost]
        //public async Task<IActionResult> ViewFile(int id)
        //{
        //    var path = (await _db.Requestwisefiles.FirstOrDefaultAsync(x => x.Requestwisefileid == id)).Filename;
        //    var provider = new FileExtensionContentTypeProvider();
        //    if (!provider.TryGetContentType(path, out var contentType))
        //    {
        //        contentType = "application/octet-stream";
        //    }
        //    var bytes = _documents.Download(id);
        //    return File(bytes, contentType, Path.GetFileName(path));
        //}

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
        public IActionResult SendMail(List<int> RequestWiseFileId , int RequestsId)
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
            var physician = _db.Physicians.ToList().Where(x => x.Regionid == regionid).ToList();
            return physician;
        }

    }
}
