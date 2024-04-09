using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using HelloDoc.Areas.PatientArea.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Globalization;

namespace DataAccess.ServiceRepository
{
    public class AllRequestDataRepository : IAllRequestDataRepository
    {
        private readonly HelloDocDbContext _db;
        private readonly IHttpContextAccessor _session;
        private readonly IBlockCaseRepository _blockcase;
        private readonly IRequestRepository _request;
        private readonly IRequestStatusLogRepository _requeststatus;
        private readonly IAdminRepository _admin;
        private readonly IRequestwisefileRepository _requestwisefile;

        public AllRequestDataRepository(HelloDocDbContext dbContext, IRequestwisefileRepository requestwisefileRepository, IHttpContextAccessor httpContextAccessor, IBlockCaseRepository blockCaseRepository, IRequestRepository requestRepository, IRequestStatusLogRepository requeststatus, IAdminRepository adminRepository)
        {
            _db = dbContext;
            _session = httpContextAccessor;
            _blockcase = blockCaseRepository;
            _request = requestRepository;
            _requeststatus = requeststatus;
            _admin = adminRepository;
            _requestwisefile = requestwisefileRepository;
        }

        public List<AllRequestDataViewModel> FilteredRequest(List<Request> requests)
        {
            List<AllRequestDataViewModel> allRequestDataViewModels = new List<AllRequestDataViewModel>();
            foreach (var item in requests)
            {
                AllRequestDataViewModel model = new AllRequestDataViewModel();
                model.PatientName = item.User.Firstname + " " + item.User.Lastname;
                model.PatientDOB = new DateOnly(Convert.ToInt32(item.User.Intyear), DateTime.ParseExact(item.User.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(item.User.Intdate));
                model.RequestorName = item.Firstname + " " + item.Lastname;
                model.RequestedDate = item.Createddate;
                model.PatientPhone = item.Requestclients.FirstOrDefault(x => x.Requestid == item.Requestid).Phonenumber;
                model.RequestorPhone = item.Phonenumber;
                model.Address = item.Requestclients.FirstOrDefault(x => x.Requestid == item.Requestid).Address;
                model.Notes = item.Requestclients.FirstOrDefault(x => x.Requestid == item.Requestid).Notes;
                model.PatientEmail = item.Requestclients.FirstOrDefault(x => x.Requestid == item.Requestid).Email;
                model.RequestorEmail = item.Email;
                model.RequestType = item.Requesttypeid;
                model.Region = item.User.Region.Name;
                model.RequestId = item.Requestid;
                model.status = _request.GetstatebyStatus(item.Status);

                if (item.Status != 1)
                {
                    if (item.Physician != null)
                    {
                        model.ProviderEmail = item.Physician.Email;
                        model.PhysicainName = item.Physician.Firstname + " " + item.Physician.Lastname;
                    }
                }
                var id = item.Requestid;
                var reqstatuslog = item.Requeststatuslogs.ToList().OrderByDescending(x => x.Createddate);
                if (reqstatuslog.Count() == 0)
                {
                    model.TransferNotes = "-";
                }
                else
                {
                    if (reqstatuslog.ElementAt(0).Transtophysician != null)
                    {
                        var date = reqstatuslog.ElementAt(0).Createddate;
                        var afterphysicanid = reqstatuslog.ElementAt(0).Transtophysicianid;
                        var afterphysician = reqstatuslog.ElementAt(0).Transtophysician.Firstname;
                        var name = "";
                        var position = "";
                        if (reqstatuslog.ElementAt(0).Adminid != null)
                        {
                            var admin = _admin.GetFirstOrDefault(x => x.Adminid == reqstatuslog.ElementAt(0).Adminid);
                            name = admin.Firstname + " " + admin.Lastname;
                            position = "Admin";

                        }
                        if (reqstatuslog.ElementAt(0).Physicianid != null)
                        {
                            var physician = _db.Physicians.FirstOrDefault(x => x.Physicianid == reqstatuslog.ElementAt(0).Physicianid);
                            name = physician.Firstname + " " + physician.Lastname;
                            position = "Physician";
                        }
                        model.TransferNotes = name + "(" + position + ") transferred case to " + afterphysician + " on " + date.ToString("dd-MM-yyyy") + " at " + date.ToString("hh:mm tt");

                    }
                    else
                    {
                        model.TransferNotes = "-";
                    }

                }


                allRequestDataViewModels.Add(model);

            }
            return allRequestDataViewModels;

        }

        public RequestDataViewModel GetRequestById(int id)
        {
            var request = _db.Requests.Include(r => r.Requestclients).FirstOrDefault(x => x.Requestid == id);
            var user = _db.Users.FirstOrDefault(x => x.Userid == request.Userid);
            RequestDataViewModel model = new RequestDataViewModel();
            model.RequestId = id;
            model.ConfirmationNo = request.Confirmationnumber;
            model.FirstName = request.Requestclients.FirstOrDefault(x => x.Requestid == id).Firstname;
            model.LastName = request.Requestclients.FirstOrDefault(x => x.Requestid == id).Lastname;
            model.PatientEmail = request.Requestclients.FirstOrDefault(x => x.Requestid == id).Email;
            model.PatientMobile = request.Requestclients.FirstOrDefault(x => x.Requestid == id).Phonenumber;
            var regionid = request.Requestclients.FirstOrDefault(x => x.Requestid == id).Regionid;
            model.Region = _db.Regions.FirstOrDefault(x => x.Regionid == regionid).Name;
            model.BusinessName = request.Requestclients.FirstOrDefault(x => x.Requestid == id).Address;
            model.RequesttypeID = request.Requesttypeid;
            model.Notes = request.Requestclients.FirstOrDefault(x => x.Requestid == id).Notes;
            model.PatientDOB = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            model.Status = request.Status;
            return model;
        }

        public RequestNotesViewModel GetNotesById(int id)
        {
            RequestNotesViewModel model = new RequestNotesViewModel();
            model.RequestId = id;
            var reqnotes = _db.Requestnotes.FirstOrDefault(x => x.Requestid == id);
            if (reqnotes == null)
            {
                model.PhysicianNotes = "-";
                model.AdminNotes = "-";
            }
            else
            {
                model.PhysicianNotes = reqnotes.Physiciannotes;
                model.AdminNotes = reqnotes.Adminnotes;
            }
            var reqstatuslog = _requeststatus.GetStatusbyId(id).Where(x => x.Transtophysicianid != null).ToList().OrderBy(x => x.Createddate);
            List<string> notes = new List<string>();
            if (reqstatuslog == null)
            {
                notes.Add("-");
            }
            else
            {
                foreach (var note in reqstatuslog)
                {
                    var date = note.Createddate;
                    var name = "";
                    var position = "";
                    var afterphysicanid = note.Transtophysicianid;
                    var afterphysician = _db.Physicians.FirstOrDefault(x => x.Physicianid == afterphysicanid).Firstname;
                    if (note.Adminid != null)
                    {
                        var admin = _admin.GetFirstOrDefault(x => x.Adminid == note.Adminid);
                        name = admin.Firstname + " " + admin.Lastname;
                        position = "Admin";

                    }
                    if (note.Physicianid != null)
                    {
                        var physician = _db.Physicians.FirstOrDefault(x => x.Physicianid == note.Physicianid);
                        name = physician.Firstname + " " + physician.Lastname;
                        position = "Physician";
                    }
                    var transfernote = name + "(" + position + ") transferred case to " + afterphysician + " on " + date.ToString("dd-MM-yyyy") + " at " + date.ToString("hh:mm tt");
                    notes.Add(transfernote);
                }

            }
            model.TransferNotes = notes;
            return model;
        }

        public void SaveAdminNotes(int id, RequestNotesViewModel model)
        {
            var curr = _db.Requestnotes.FirstOrDefault(x => x.Requestid == id);
            if (curr != null)
            {
                curr.Adminnotes = model.AdminNotes;
                _db.Requestnotes.Update(curr);
            }
            else
            {
                var adminid = _session.HttpContext.Session.GetInt32("AdminId");
                var aspid = _db.Admins.FirstOrDefault(x => x.Adminid == adminid).Aspnetuserid;
                Requestnote requestnote = new Requestnote();

                requestnote.Requestid = id;
                requestnote.Adminnotes = model.AdminNotes;
                requestnote.Createdby = aspid;
                requestnote.Createddate = DateTime.Now;
                _db.Requestnotes.Add(requestnote);
            }
            _db.SaveChanges();
        }

        public byte[] DownloadExcle(List<AllRequestDataViewModel> model)
        {
            using (var workbook = new XSSFWorkbook())
            {
                ISheet sheet = workbook.CreateSheet("FilteredRecord");
                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Sr No.");
                headerRow.CreateCell(1).SetCellValue("Request Id");
                headerRow.CreateCell(2).SetCellValue("Patient Name");
                headerRow.CreateCell(3).SetCellValue("Patient DOB");
                headerRow.CreateCell(4).SetCellValue("RequestorName");
                headerRow.CreateCell(5).SetCellValue("RequestedDate");
                headerRow.CreateCell(6).SetCellValue("PatientPhone");
                headerRow.CreateCell(7).SetCellValue("TransferNotes");
                headerRow.CreateCell(8).SetCellValue("RequestorPhone");
                headerRow.CreateCell(9).SetCellValue("RequestorEmail");
                headerRow.CreateCell(10).SetCellValue("Address");
                headerRow.CreateCell(11).SetCellValue("Notes");
                headerRow.CreateCell(12).SetCellValue("ProviderEmail");
                headerRow.CreateCell(13).SetCellValue("PatientEmail");
                headerRow.CreateCell(14).SetCellValue("RequestType");
                headerRow.CreateCell(15).SetCellValue("Region");
                headerRow.CreateCell(16).SetCellValue("PhysicainName");
                headerRow.CreateCell(17).SetCellValue("Status");

                for (int i = 0; i < model.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(i + 1);
                    row.CreateCell(1).SetCellValue(model[i].RequestId);
                    row.CreateCell(2).SetCellValue(model[i].PatientName);
                    row.CreateCell(3).SetCellValue(model[i].PatientDOB);
                    row.CreateCell(4).SetCellValue(model[i].RequestorName);
                    row.CreateCell(5).SetCellValue(model[i].RequestedDate);
                    row.CreateCell(6).SetCellValue(model[i].PatientPhone);
                    row.CreateCell(7).SetCellValue(model[i].TransferNotes);
                    row.CreateCell(8).SetCellValue(model[i].RequestorPhone);
                    row.CreateCell(9).SetCellValue(model[i].RequestorEmail);
                    row.CreateCell(10).SetCellValue(model[i].Address);
                    row.CreateCell(11).SetCellValue(model[i].Notes);
                    row.CreateCell(12).SetCellValue(model[i].ProviderEmail);
                    row.CreateCell(13).SetCellValue(model[i].PatientEmail);
                    row.CreateCell(14).SetCellValue(model[i].RequestType);
                    row.CreateCell(15).SetCellValue(model[i].Region);
                    row.CreateCell(16).SetCellValue(model[i].PhysicainName);
                    row.CreateCell(17).SetCellValue(model[i].status);
                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        public void EditEmailPhone(RequestDataViewModel model)
        {
            var reqclient = _db.Requestclients.FirstOrDefault(x => x.Requestid == model.RequestId);
            reqclient.Email = model.PatientEmail;
            reqclient.Phonenumber = model.PatientMobile;
            _db.Requestclients.Update(reqclient);
            _db.SaveChanges();
        }

        public RequestViewUploadsViewModel GetDocumentByRequestId(int id)
        {
            var request = _db.Requests.Include(r => r.Requestwisefiles).Include(r => r.User).Include(r => r.Requestclients).FirstOrDefault(x => x.Requestid == id);
            RequestViewUploadsViewModel model = new RequestViewUploadsViewModel();
            var user = request.User;
            model.RequestsId = id;
            model.confirmation = request.Confirmationnumber;
            model.requestwisefiles = request.Requestwisefiles.ToList().Where(x => x.Isdeleted == null).ToList();
            model.patientname = user.Firstname + " " + user.Lastname;
            model.FirstName = request.Requestclients.ElementAt(0).Firstname;
            model.LastName = request.Requestclients.ElementAt(0).Lastname;
            model.PatientEmail = request.Requestclients.ElementAt(0).Email;
            model.PatientMobile = request.Requestclients.ElementAt(0).Phonenumber;
            model.PatientDOB = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            return model;
        }

        public void AddRequestasAdmin(FamilyRequestViewModel model)
        {
            var aspnetuser = _db.Aspnetusers.FirstOrDefault(m => m.Email == model.Email);
            var user = _db.Users.FirstOrDefault(x => x.Email == model.Email);

            var region = _db.Regions.FirstOrDefault(x => x.Regionid == user.Regionid);
            var requestcount = (from m in _db.Requests where m.Createddate.Date == DateTime.Now.Date select m).ToList();
            if (aspnetuser != null)
            {
                Request request = new Request
                {
                    Requesttypeid = 5,
                    Userid = user.Userid,
                    Firstname = model.F_FirstName,
                    Lastname = model.F_LastName,
                    Email = model.F_Email,
                    Phonenumber = model.F_Phone,
                    Status = model.Status,
                    Createddate = DateTime.Now,
                    Relationname = model.Relation,
                    Confirmationnumber = (region.Abbreviation.Substring(0, 2) + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + model.LastName.Substring(0, 2) + model.FirstName.Substring(0, 2) + requestcount.Count().ToString().PadLeft(4, '0')).ToUpper(),
                    User = user,
                };
                _db.Add(request);
                _db.SaveChanges();
                Requestclient requestclient = new Requestclient
                {
                    Notes = model.Symptoms,
                    Requestid = request.Requestid,
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Email = model.Email,
                    Phonenumber = model.Phone,
                    State = model.State,
                    Street = model.Street,
                    City = model.City,
                    Zipcode = model.ZipCode,
                    Address = model.Room + " , " + model.Street + " , " + model.City + " , " + model.State,
                    Intdate = model.BirthDate.Day,
                    Intyear = model.BirthDate.Year,
                    Strmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.BirthDate.Month),
                    Regionid = (int)user.Regionid,
                };
                _db.Add(requestclient);
                _db.SaveChanges();
                if (model.Upload != null)
                {
                    _requestwisefile.Add(request.Requestid, model.Upload);
                }
            }
        }
    }
}












//var allRequestDataViewModels = from user in _db.Users
//                               join req in _db.Requests on user.Userid equals req.Userid
//                               where req.Status == status
//                               orderby req.Createddate descending
//                               select new AllRequestDataViewModel
//                               {
//                                   PatientName = user.Firstname + " " + user.Lastname,
//                                   PatientDOB = new DateOnly(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate)),
//                                   RequestorName = req.Firstname + " " + req.Lastname,
//                                   RequestedDate = req.Createddate,
//                                   PatientPhone = user.Mobile,
//                                   RequestorPhone = req.Phonenumber,
//                                   Address = req.Requestclients.FirstOrDefault(x => x.Requestid == req.Requestid).Address,
//                                   Notes = req.Requestclients.FirstOrDefault(x => x.Requestid == req.Requestid).Notes,
//                                   ProviderEmail = _db.Physicians.FirstOrDefault(x => x.Physicianid == req.Physicianid).Email,
//                                   PatientEmail = user.Email,
//                                   RequestorEmail = req.Email,
//                                   RequestType = req.Requesttypeid ,
//                               };

//return allRequestDataViewModels.ToList();