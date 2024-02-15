using Azure.Core;
using HelloDoc.Areas.Patient.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace HelloDoc.Areas.Patient.DataController
{
    public class DashboardController : Controller
    {

        public readonly HelloDocDbContext _context;
        public DashboardController(HelloDocDbContext context)
        {
            _context = context;
        }
        public void AddPatientRequestWiseFile(List<IFormFile> formFile, int requestid)
        {
            foreach (var file in formFile)
            {
                string filename = requestid.ToString() + " _ " + file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", filename);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var data3 = new Requestwisefile()
                {
                    Requestid = requestid,
                    Filename = path,
                    Createddate = DateTime.Now,
                };

                _context.Requestwisefiles.Add(data3);

            }
            _context.SaveChanges();
        }
        public async Task<IActionResult> Dashboard(PatientDashboardViewModel patientDashboardviewmodel)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int id = (int)HttpContext.Session.GetInt32("UserId");
                PatientDashboardViewModel patientDashboard = new PatientDashboardViewModel();
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
                var request = from m in _context.Requests
                              where m.Userid == id
                              select m;
                patientDashboard.User = user;
                patientDashboard.Requests = request.ToList();

                DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
                patientDashboard.birthdate = date;
                List<Requestwisefile> files;
                if (patientDashboardviewmodel.RequestsId == 0)
                {
                     files = (from m in _context.Requestwisefiles select m).ToList();
                }
                else
                {
                     files = (from m in _context.Requestwisefiles where m.Requestid==patientDashboardviewmodel.RequestsId select m).ToList();
                    patientDashboard.RequestsId = patientDashboardviewmodel.RequestsId;
                }
                patientDashboard.requestwisefiles = files;
                patientDashboard.showdocument = patientDashboardviewmodel.showdocument;
                return View(patientDashboard);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public async Task<IActionResult> Edit(PatientDashboardViewModel model)
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);

            user.Firstname = model.User.Firstname;
            user.Lastname = model.User.Lastname;
            user.Email = model.User.Email;
            user.Street = model.User.Street;
            user.City = model.User.City;
            user.State = model.User.State;
            user.Zip = model.User.Zip;
            user.Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.birthdate.Month);
            user.Intdate = model.birthdate.Day;
            user.Intyear = model.birthdate.Year;
            user.Modifiedby = model.User.Email;
            user.Modifieddate = DateTime.Now;

            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Document(PatientDashboardViewModel model)
        {
            var requestwisefile = (from m in _context.Requestwisefiles where m.Requestid == model.RequestsId select m).ToList();
            int id = (int)HttpContext.Session.GetInt32("UserId");
            PatientDashboardViewModel patientDashboard = new PatientDashboardViewModel();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);
            var request = from m in _context.Requests
                          where m.Userid == id
                          select m;
            patientDashboard.User = user;
            patientDashboard.Requests = request.ToList();

            DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            patientDashboard.birthdate = date;

            patientDashboard.requestwisefiles = requestwisefile;
            patientDashboard.showdocument = model.showdocument;
            return RedirectToAction("Dashboard", patientDashboard);
        }
        [HttpPost]
        public async Task<IActionResult> AddDocument(PatientDashboardViewModel model)
        {
            PatientDashboardViewModel patientDashboard = new PatientDashboardViewModel();
            patientDashboard.RequestsId = model.RequestsId;
            patientDashboard.showdocument = model.showdocument;

            if (model.Upload != null)
            {
                AddPatientRequestWiseFile(model.Upload, model.RequestsId);
            }
            return RedirectToAction("Dashboard" , patientDashboard);
        }
        [HttpPost]
        public async Task<IActionResult> Download(PatientDashboardViewModel dashedit)
        {
            var checkbox = Request.Form["downloadselect"].ToList();
            var zipname = dashedit.RequestsId.ToString() + "_"+ DateTime.Now + ".zip";
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var item in checkbox)
                    {
                        var s = Int32.Parse(item);
                        var file = await _context.Requestwisefiles.FirstOrDefaultAsync(x => x.Requestwisefileid == s);
                        var path = file.Filename;
                        var bytes = await System.IO.File.ReadAllBytesAsync(path);
                        var zipEntry = zipArchive.CreateEntry(file.Filename.Split("\\Documents\\")[1], CompressionLevel.Fastest);
                        using (var zipStream = zipEntry.Open())
                        {
                            await zipStream.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                }
                memoryStream.Position = 0; // Reset the position
                return File(memoryStream.ToArray(), "application/zip", zipname, enableRangeProcessing:true);
            }
        }


        public async Task<IActionResult> Download(int id)
        {
            var path = (await _context.Requestwisefiles.FirstOrDefaultAsync(x => x.Requestwisefileid == id)).Filename;
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            return File(bytes, contentType, Path.GetFileName(path));
        }


        [HttpPost]
        public async Task<IActionResult> RequestByPatient(PatientDashboardViewModel patientDashboardView)
        {
            var radio = Request.Form["selectrequesttype"];
            return View();
        }
    }
}
