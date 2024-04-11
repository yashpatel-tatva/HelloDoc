using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Utilities.Encoders;
using System.Buffers.Text;
using System.Configuration.Provider;
using System.Reflection;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AdminRecordsTab : Controller
    {
        private readonly HelloDocDbContext _db;
        private readonly IBlockCaseRepository _blockCase;
        private readonly IRequestRepository _request;
        public AdminRecordsTab(HelloDocDbContext helloDocDbContext, IBlockCaseRepository blockCaseRepository, IRequestRepository request)
        {
            _db = helloDocDbContext;
            _blockCase = blockCaseRepository;
            _request = request;
        }

        [Area("AdminArea")]
        public IActionResult PatientHistory()
        {
            return View();
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult PatientsData(string firstname, string lastname, string emailid, string mobile, int currentpage, int pagesize)
        {
            List<PatientHistryViewModel> models = new List<PatientHistryViewModel>();
            var users = _db.Users.Where(x => x.Isdeleted == null).ToList();
            if (firstname != null)
            {
                firstname = firstname.Replace(" ", "").ToLower();
                users = users.Where(x => x.Firstname.ToLower().Contains(firstname)).ToList();
            }
            if (lastname != null)
            {
                lastname = lastname.Replace(" ", "").ToLower();
                users = users.Where(x => x.Lastname.ToLower().Contains(lastname)).ToList();
            }
            if (emailid != null)
            {
                emailid = emailid.Replace(" ", "").ToLower();
                users = users.Where(x => x.Email.ToLower().Contains(emailid)).ToList();
            }
            if (mobile != null)
            {
                mobile = mobile.Replace(" ", "").ToLower();
                users = users.Where(x => x.Mobile != null && x.Mobile.ToLower().Contains(mobile)).ToList();
            }
            users = users.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            foreach (var item in users)
            {
                models.Add(new PatientHistryViewModel
                {
                    UserId = item.Userid,
                    Firstname = item.Firstname,
                    Lastname = item.Lastname,
                    Email = item.Email,
                    PhoneNumber = item.Mobile,
                    Address = item.Street + " , " + item.City + " , " + item.State,

                });
            }
            return PartialView("_PatientHistory", models);
        }

        [Area("AdminArea")]
        [HttpPost]
        public int PatientDataCount(string firstname, string lastname, string emailid, string mobile)
        {
            var users = _db.Users.Where(x => x.Isdeleted == null).ToList();
            if (firstname != null)
            {
                firstname = firstname.Replace(" ", "").ToLower();
                users = users.Where(x => x.Firstname.ToLower().Contains(firstname)).ToList();
            }
            if (lastname != null)
            {
                lastname = lastname.Replace(" ", "").ToLower();
                users = users.Where(x => x.Lastname.ToLower().Contains(lastname)).ToList();
            }
            if (emailid != null)
            {
                emailid = emailid.Replace(" ", "").ToLower();
                users = users.Where(x => x.Email.ToLower().Contains(emailid)).ToList();
            }
            if (mobile != null)
            {
                mobile = mobile.Replace(" ", "").ToLower();
                users = users.Where(x => x.Mobile != null && x.Mobile.ToLower().Contains(mobile)).ToList();
            }
            return users.Count();
        }


        [Area("AdminArea")]
        [HttpPost]
        public IActionResult PatientRecords(int userid)
        {
            List<PatientRecordViewModel> records = new List<PatientRecordViewModel>();
            var request = _request.GetRequestsbyState("all").Where(x => x.Userid == userid);
            foreach (var record in request)
            {
                PatientRecordViewModel model = new PatientRecordViewModel();
                model.RequestId = record.Requestid;
                model.RequestorName = record.Firstname + ' ' + record.Lastname;
                model.Createddate = record.Createddate;
                model.Confirmation = record.Confirmationnumber;
                if (record.Physicianid != null)
                {
                    model.Providername = record.Physician.Firstname + " " + record.Physician.Lastname;
                }
                if (record.Requeststatuslogs != null)
                {
                    if (record.Requeststatuslogs.Where(x => x.Status == 6).LastOrDefault() != null)
                    {
                        model.Conculdedate = record.Requeststatuslogs.Where(x => x.Status == 6).LastOrDefault().Createddate;
                    }
                }
                if (record.Status == 1) model.Status = "New";
                if (record.Status == 2) model.Status = "Pending";
                if (record.Status == 4 || record.Status == 5) model.Status = "Active";
                if (record.Status == 6) model.Status = "Conclude";
                if (record.Status == 3 || record.Status == 7 || record.Status == 8) model.Status = "ToClose";
                if (record.Status == 9) model.Status = "Unpaid";
                model.IsFinalReport = false;
                model.DocumentCount = record.Requestwisefiles.Count();
                records.Add(model);
            }
            return View(records);
        }

        [Area("AdminArea")]
        public IActionResult SearchRecords()
        {
            return View();
        }
        [Area("AdminArea")]
        public IActionResult EmailLogs()
        {
            return View();
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EmailLogsdata(int role, string rname, string email, string createddate, string senddate, int currentpage, int pagesize, bool order)
        {
            if (rname != null) { rname = rname.Replace(" ", ""); rname = rname.ToLower(); }
            if (email != null) { email = email.Replace(" ", ""); email = email.ToLower(); }
            if (createddate != null) createddate = createddate.Replace(" ", "");
            if (senddate != null) senddate = senddate.Replace(" ", "");

            List<EmailSMSLogsViewModel> model = new List<EmailSMSLogsViewModel>();
            var emaillogs = _db.Emaillogs.ToList();
            if (role != 0)
            {
                emaillogs = emaillogs.Where(x => x.Roleid == role).ToList();
            }
            if (email != null)
            {
                emaillogs = emaillogs.Where(x => x.Emailid.ToLower().Contains(email)).ToList();
            }
            if (createddate != null)
            {
                emaillogs = emaillogs.Where(x => x.Createdate.ToString("yyyy-MM-dd") == createddate).ToList();
            }
            if (senddate != null)
            {
                emaillogs = emaillogs.Where(x => x.Sentdate.HasValue && x.Sentdate.Value.ToString("yyyy-MM-dd") == senddate).ToList();
            }
            if (rname != null)
            {
                emaillogs = emaillogs.Where(x => x.Emailid.Split('@')[0].ToLower().Contains(rname.ToLower())).ToList();
            }
            if (order == true)
            {
                emaillogs = emaillogs.OrderBy(x => x.Createdate).ToList();
            }
            else
            {
                emaillogs = emaillogs.OrderByDescending(x => x.Createdate).ToList();
            }
            emaillogs = emaillogs.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            foreach (var log in emaillogs)
            {
                EmailSMSLogsViewModel em = new EmailSMSLogsViewModel();
                //if (log.Requestid != null)
                //{
                //    em.Recipient = _db.Requestclients.FirstOrDefault(x => x.Requestid == log.Requestid).Firstname +
                //        " " + _db.Requestclients.FirstOrDefault(x => x.Requestid == log.Requestid).Lastname;
                //}
                //else
                //{
                //    em.Recipient = log.Emailid.Split('@')[0];
                //}
                em.Recipient = log.Emailid.Split('@')[0];
                em.Action = log.Subjectname;
                em.RoleName = _db.Aspnetroles.FirstOrDefault(x => x.Id == log.Roleid.ToString()).Name;
                em.Email = log.Emailid;
                em.CreatedDate = log.Createdate;
                em.SentDate = (DateTime)log.Sentdate;
                em.Sent = log.Isemailsent[0];
                em.Senttries = (int)log.Senttries;
                if (log.Confirmationnumber != null)
                {
                    em.ConfirmationNo = log.Confirmationnumber;
                }
                model.Add(em);
            }

            return PartialView("_EmailLogs", model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public int EmailContbyFilter(int role, string rname, string email, string createddate, string senddate)
        {
            if (rname != null) { rname = rname.Replace(" ", ""); rname = rname.ToLower(); }
            if (email != null) { email = email.Replace(" ", ""); email = email.ToLower(); }
            if (createddate != null) createddate = createddate.Replace(" ", "");
            if (senddate != null) senddate = senddate.Replace(" ", "");

            List<EmailSMSLogsViewModel> model = new List<EmailSMSLogsViewModel>();
            var emaillogs = _db.Emaillogs.ToList();
            if (role != 0)
            {
                emaillogs = emaillogs.Where(x => x.Roleid == role).ToList();
            }
            if (email != null)
            {
                emaillogs = emaillogs.Where(x => x.Emailid.ToLower().Contains(email)).ToList();
            }
            if (createddate != null)
            {
                emaillogs = emaillogs.Where(x => x.Createdate.ToString("yyyy-MM-dd") == createddate).ToList();
            }
            if (senddate != null)
            {
                emaillogs = emaillogs.Where(x => x.Sentdate.HasValue && x.Sentdate.Value.ToString("yyyy-MM-dd") == senddate).ToList();
            }
            if (rname != null)
            {
                emaillogs = emaillogs.Where(x => x.Emailid.Split('@')[0].ToLower().Contains(rname.ToLower())).ToList();
            }

            return emaillogs.Count();
        }

        /// <summary>
        /// SMSLOGS
        /// </summary>
        /// <returns></returns>
        /// 

        [Area("AdminArea")]
        public IActionResult SmsLogs()
        {
            return View();
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult SMSLogsdata(int role, string rname, string mobile, string createddate, string senddate, int currentpage, int pagesize)
        {
            if (rname != null) { rname = rname.Replace(" ", ""); rname = rname.ToLower(); }
            if (mobile != null) { mobile = mobile.Replace(" ", ""); mobile = mobile.ToLower(); }
            if (createddate != null) createddate = createddate.Replace("ya", "");
            if (senddate != null) senddate = senddate.Replace("ya", "");

            List<EmailSMSLogsViewModel> model = new List<EmailSMSLogsViewModel>();
            var smslogs = _db.Smslogs.ToList();
            if (role != 0)
            {
                smslogs = smslogs.Where(x => x.Roleid == role).ToList();
            }
            if (mobile != null)
            {
                smslogs = smslogs.Where(x => x.Mobilenumber.Contains(mobile)).ToList();
            }
            if (createddate != null)
            {
                smslogs = smslogs.Where(x => x.Createdate.ToString("yyyy-MM-dd") == createddate).ToList();
            }
            if (senddate != null)
            {
                smslogs = smslogs.Where(x => x.Sentdate.HasValue && x.Sentdate.Value.ToString("yyyy-MM-dd") == senddate).ToList();
            }
            smslogs = smslogs.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            foreach (var log in smslogs)
            {
                EmailSMSLogsViewModel em = new EmailSMSLogsViewModel();
                //if (log.Requestid != null)
                //{
                //    em.Recipient = _db.Requestclients.FirstOrDefault(x => x.Requestid == log.Requestid).Firstname +
                //        " " + _db.Requestclients.FirstOrDefault(x => x.Requestid == log.Requestid).Lastname;
                //}
                //else
                //{
                //    em.Recipient = log.Emailid.Split('@')[0];
                //}
                //em.Recipient = log.Emailid.Split('@')[0];
                em.Action = log.Smstemplate;
                em.RoleName = _db.Aspnetroles.FirstOrDefault(x => x.Id == log.Roleid.ToString()).Name;
                em.Mobile = log.Mobilenumber;
                em.CreatedDate = log.Createdate;
                em.SentDate = (DateTime)log.Sentdate;
                em.Sent = log.Issmssent[0];
                em.Senttries = (int)log.Senttries;
                if (log.Confirmationnumber != null)
                {
                    em.ConfirmationNo = log.Confirmationnumber;
                }
                model.Add(em);
            }

            return PartialView("_EmailLogs", model);
        }

        [Area("AdminArea")]
        [HttpPost]
        public int SMSContbyFilter(int role, string rname, string mobile, string createddate, string senddate)
        {
            if (rname != null) { rname = rname.Replace(" ", ""); rname = rname.ToLower(); }
            if (mobile != null) { mobile = mobile.Replace(" ", ""); mobile = mobile.ToLower(); }
            if (createddate != null) createddate = createddate.Replace("ya", "");
            if (senddate != null) senddate = senddate.Replace("ya", "");

            List<EmailSMSLogsViewModel> model = new List<EmailSMSLogsViewModel>();
            var smslogs = _db.Smslogs.ToList();
            if (role != 0)
            {
                smslogs = smslogs.Where(x => x.Roleid == role).ToList();
            }
            if (mobile != null)
            {
                smslogs = smslogs.Where(x => x.Mobilenumber.Contains(mobile)).ToList();
            }
            if (createddate != null)
            {
                smslogs = smslogs.Where(x => x.Createdate.ToString("yyyy-MM-dd") == createddate).ToList();
            }
            if (senddate != null)
            {
                smslogs = smslogs.Where(x => x.Sentdate.HasValue && x.Sentdate.Value.ToString("yyyy-MM-dd") == senddate).ToList();
            }

            return smslogs.Count();
        }

        /// <summary>
        /// BlockHistory
        /// </summary>
        /// <returns></returns>

        [Area("AdminArea")]
        public IActionResult BlockHistory()
        {
            return View();
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult BlockHistorydata(string pname, string email, string createddate, string mobile, int currentpage, int pagesize, bool order)
        {
            if (pname != null) { pname = pname.Replace(" ", ""); pname = pname.ToLower(); }
            if (email != null) { email = email.Replace(" ", ""); email = email.ToLower(); }
            if (mobile != null) { mobile = mobile.Replace(" ", ""); mobile = mobile.ToLower(); }
            if (createddate != null) { createddate = createddate.Replace(" ", ""); createddate = createddate.ToLower(); }
            var blockhistory = _blockCase.GetBlockHistory();

            if (pname != null)
            {
                blockhistory = blockhistory.Where(x => x.Name.ToLower().Contains(pname)).ToList();
            }
            if (email != null)
            {
                blockhistory = blockhistory.Where(x => x.Email.ToLower().Contains(email)).ToList();
            }
            if (mobile != null)
            {
                blockhistory = blockhistory.Where(x => x.Mobile.Contains(mobile)).ToList();
            }
            if (createddate != null)
            {
                blockhistory = blockhistory.Where(x => x.CreatedDate.ToString("yyyy-MM-dd") == createddate).ToList();
            }
            if (order)
            {
                blockhistory = blockhistory.OrderBy(x => x.CreatedDate).ToList();
            }
            else
            {
                blockhistory = blockhistory.OrderByDescending(x => x.CreatedDate).ToList();
            }
            blockhistory = blockhistory.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            return PartialView("_BlockHistory", blockhistory);
        }

        [Area("AdminArea")]
        [HttpPost]
        public int BlockHistorydataCount(string pname, string email, string createddate, string mobile)
        {
            if (pname != null) { pname = pname.Replace(" ", ""); pname = pname.ToLower(); }
            if (email != null) { email = email.Replace(" ", ""); email = email.ToLower(); }
            if (mobile != null) { mobile = mobile.Replace(" ", ""); mobile = mobile.ToLower(); }
            if (createddate != null) { createddate = createddate.Replace(" ", ""); createddate = createddate.ToLower(); }
            var blockhistory = _blockCase.GetBlockHistory();

            if (pname != null)
            {
                blockhistory = blockhistory.Where(x => x.Name.ToLower().Contains(pname)).ToList();
            }
            if (email != null)
            {
                blockhistory = blockhistory.Where(x => x.Email.ToLower().Contains(email)).ToList();
            }
            if (mobile != null)
            {
                blockhistory = blockhistory.Where(x => x.Mobile.Contains(mobile)).ToList();
            }
            if (createddate != null)
            {
                blockhistory = blockhistory.Where(x => x.CreatedDate.ToString("yyyy-MM-dd") == createddate).ToList();
            }
            return blockhistory.Count();
        }


        public void UnblockCase(int id)
        {
            _blockCase.UnblockThis(id);
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult SearchRecordsdata(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile, int currentpage, int pagesize, bool order)
        {
            List<SearchRecodsViewModel> models = new List<SearchRecodsViewModel>();
            models = _request.GetFilterdData(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile, currentpage, pagesize, order);
            return PartialView("_SearchRecords", models);
        }

        [Area("AdminArea")]
        [HttpPost]
        public int SearchRecordsCount(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile)
        {
            return _request.SearchRecordsCount(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile);
        }

        [Area("AdminArea")]
        [HttpPost]
        public string SearchRecordsdatatoexcle(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile, bool order)
        {
            List<SearchRecodsViewModel> models = new List<SearchRecodsViewModel>();
            var record = _request.GetFilterdDatatoexcle(selectstatus, patientname, selecttype, fromdate, todate, providername, emailid, mobile, order);
            //string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //var strDate = DateTime.Now.ToString("yyyyMMdd");
            //string filename = $"All Request_{strDate}.xlsx";

            return System.Convert.ToBase64String(record);
        }
        [Area("AdminArea")]
        [HttpPost]
        public void DeleteRecord(int requestid)
        {
            _request.DeleteThisRequest(requestid);
        }
    }
}
