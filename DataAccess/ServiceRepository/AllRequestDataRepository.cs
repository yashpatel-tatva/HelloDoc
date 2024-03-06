using DataAccess.Repository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Mvc;
using static HelloDoc.Areas.PatientArea.ViewModels.PatientDashboardViewModel;
using DataAccess.Repository.IRepository;

namespace DataAccess.ServiceRepository
{
    public class AllRequestDataRepository : IAllRequestDataRepository
    {
        private readonly HelloDocDbContext _db;
        private readonly IHttpContextAccessor _session;
        private readonly IBlockCaseRepository _blockcase;
        private readonly IRequestRepository _request;

        public AllRequestDataRepository(HelloDocDbContext dbContext, IHttpContextAccessor httpContextAccessor, IBlockCaseRepository blockCaseRepository , IRequestRepository requestRepository)
        {
            _db = dbContext;
            _session = httpContextAccessor;
            _blockcase = blockCaseRepository;
            _request = requestRepository;
        }

        public List<AllRequestDataViewModel> Status(int status)
        {
            List<AllRequestDataViewModel> allRequestDataViewModels = new List<AllRequestDataViewModel>();
            var requests = _db.Requests.Include(r => r.User).Include(r => r.Requestclients).Include(r => r.Physician).Include(r => r.User.Region).Include(r => r.Requeststatuslogs).ToList().Where(r => r.Status == status);
            //if(status == 0)
            //{
            //     requests = _db.Requests.Include(r => r.User).Include(r => r.Requestclients).Include(r => r.Physician).Include(r => r.User.Region).Include(r => r.Requeststatuslogs);
            //}
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

                if (status != 1)
                {
                    if (item.Physician != null)
                    {
                        model.ProviderEmail = item.Physician.Email;
                        model.PhysicainName = item.Physician.Firstname + " " + item.Physician.Lastname;
                    }
                }
                var id = item.Requestid;
                var reqstatuslog = item.Requeststatuslogs;
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
                        model.TransferNotes = "Admin transferred case to " + afterphysician + " on " + date.ToString("dd-MM-yyyy") +  " at " + date.ToString("hh:mm tt");
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
            model.BuisnessName = request.Requestclients.FirstOrDefault(x => x.Requestid == id).Address;
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
            var reqstatuslog = _db.Requeststatuslogs.FirstOrDefault(x => x.Requestid == id);
            if (reqstatuslog == null)
            {
                model.TransferNotes = "-";
            }
            else
            {
                if(reqstatuslog.Transtophysicianid != null) {
                    var date = _db.Requeststatuslogs.FirstOrDefault(x => x.Requestid == id).Createddate;
                    var afterphysicanid = _db.Requeststatuslogs.FirstOrDefault(y => y.Requestid == id).Transtophysicianid;
                    var afterphysician = _db.Physicians.FirstOrDefault(x => x.Physicianid == afterphysicanid).Firstname;
                    model.TransferNotes = "Admin transferred case to " + afterphysician + " on " + date.ToString("dd-MM-yyyy") + " at " + date.ToString("hh:mm tt");
                }
            }
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

        public byte[] DownloadExcle(string status)
        {
            int[] statuses = new int[9];
            if (status == "status-new")
            {
                statuses[0] = 1;
            }
            else if (status == "status-pending")
            {
                statuses[0] = 2;
            }
            else if (status == "status-active")
            {
                statuses[0] = 4;
                statuses[1] = 5;

            }
            else if (status == "status-conclude")
            {
                statuses[0] = 6;
            }
            else if (status == "status-toclose")
            {
                statuses[0] = 3;
                statuses[1] = 7;
                statuses[2] = 8;
            }
            else if (status == "status-unpaid")
            {
                statuses[0] = 9;
            }
            else if (status == "all")
            {
                statuses[0] = 1;
                statuses[1] = 2;
                statuses[2] = 3;
                statuses[3] = 4;
                statuses[4] = 5;
                statuses[5] = 6;
                statuses[6] = 7;
                statuses[7] = 8;
                statuses[8] = 9;
            }

            List<AllRequestDataViewModel> model = new List<AllRequestDataViewModel>();
            foreach (int s in statuses)
            {
                List<AllRequestDataViewModel> model1 = Status(s);
                model = model.Concat(model1).ToList();
            }

            using (var workbook = new XSSFWorkbook())
            {
                ISheet sheet = workbook.CreateSheet(status);
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
            var request = _db.Requests.Include(r=>r.Requestwisefiles).Include(r=>r.User).FirstOrDefault(x=>x.Requestid == id);
            RequestViewUploadsViewModel model = new RequestViewUploadsViewModel();

            model.RequestsId = id;
            model.confirmation = request.Confirmationnumber;
            model.requestwisefiles = request.Requestwisefiles.ToList().Where(x => x.Isdeleted == null).ToList();
            model.patientname = request.User.Firstname + " " + request.User.Lastname;
            return model;
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