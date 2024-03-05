using Azure.Core;
using HelloDoc.Areas.PatientArea.ViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace HelloDoc.Areas.PatientArea.DataController
{
    public class DashboardController : Controller
    {

        public readonly HelloDocDbContext _context;
        public DashboardController(HelloDocDbContext context)
        {
            _context = context;
        }
        [Area("PatientArea")]
        public void AddPatientRequestWiseFile(List<IFormFile> formFile, int requestid)
        {
            foreach (var file in formFile)
            {
                string filename = file.FileName;
                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
                string extension = Path.GetExtension(filename);
                string filewith = filenameWithoutExtension +"_"+ DateTime.Now.ToString("dd`MM`yyyy`HH`mm`ss") + extension;

                //string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HalloDoc Request Documents", requestid.ToString());
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", requestid.ToString());

                if (!Directory.Exists(directoryPath))
                {
                    // Create the directory
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, filewith);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }


                var data3 = new Requestwisefile()
                {
                    Requestid = requestid,
                    Filename = filePath,
                    Createddate = DateTime.Now,
                };

                _context.Requestwisefiles.Add(data3);

            }
            _context.SaveChanges();
        }
        [Area("PatientArea")]

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
                    files = (from m in _context.Requestwisefiles where m.Isdeleted == null select m).ToList();
                }
                else
                {
                    files = (from m in _context.Requestwisefiles where m.Requestid == patientDashboardviewmodel.RequestsId && m.Isdeleted == null select m).ToList();
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
        [Area("PatientArea")]

        public async Task<IActionResult> Edit(PatientDashboardViewModel model)
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Userid == id);

            user.Firstname = model.User.Firstname;
            user.Lastname = model.User.Lastname;
            user.Email = model.User.Email;
            user.Mobile = model.User.Mobile;
            user.Street = model.User.Street;
            user.City = model.User.City;
            user.State = model.User.State;
            user.Zip = model.User.Zip;
            user.Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.birthdate.Month);
            user.Intdate = model.birthdate.Day;
            user.Intyear = model.birthdate.Year;
            user.Modifiedby = model.User.Email;
            user.Modifieddate = DateTime.Now;
            HttpContext.Session.SetString("UserName", user.Firstname + " " + user.Lastname);

            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "Dashboard");
        }
        [Area("PatientArea")]

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

            patientDashboard.requestwisefiles = requestwisefile.Where(x => x.Isdeleted == null).ToList(); ;
            patientDashboard.showdocument = model.showdocument;
            return RedirectToAction("Dashboard", patientDashboard);
        }
        [Area("PatientArea")]
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
            return RedirectToAction("Dashboard", patientDashboard);
        }
        [Area("PatientArea")]
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
                        var file = await _context.Requestwisefiles.FirstOrDefaultAsync(x => x.Requestwisefileid == s);
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

        [Area("PatientArea")]

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

        [Area("PatientArea")]

        [HttpPost]
        public async Task<IActionResult> RequestByPatient(PatientDashboardViewModel patientDashboardView)
        {
            var radio = Request.Form["selectrequesttype"];
            if (radio == "me")
            {
                return RedirectToAction("PatientRequestForMe");
            }
            else
            {
                return RedirectToAction("PatientRequestForSomeoneelse");
            }
        }
        [Area("PatientArea")]

        public async Task<IActionResult> PatientRequestForMe()
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id);
            PatientRequestViewModel model = new PatientRequestViewModel();
            model.FirstName = user.Firstname;
            model.LastName = user.Lastname;
            model.BirthDate = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            model.Email = user.Email;
            model.PhoneNumber = user.Mobile ;
            return View(model);
        }
        [Area("PatientArea")]
        public async Task<IActionResult> PatientRequestForSomeoneelse()
        {
            int id = (int)HttpContext.Session.GetInt32("UserId");
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id);
            FamilyRequestViewModel model = new FamilyRequestViewModel();
            model.F_FirstName = user.Firstname;
            model.F_LastName = user.Lastname;
            model.F_Email = user.Email;
            model.F_Phone = user.Mobile;
            return View(model);
        }
    }
}
