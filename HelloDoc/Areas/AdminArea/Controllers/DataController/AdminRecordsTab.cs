﻿using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using NPOI.SS.Formula.Functions;
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
        public IActionResult PatientRecords()
        {
            return View();
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
            var requests = _request.GetRequestsbyState(selectstatus);

            if (patientname != null)
            {
                patientname = patientname.Replace(" ", ""); patientname = patientname.ToLower();
                requests = requests.Where(x => (x.Requestclients.FirstOrDefault().Firstname + x.Requestclients.FirstOrDefault().Lastname).ToLower().Contains(patientname)).ToList();
            }
            if (selecttype != 0)
            {
                requests = requests.Where(x => x.Requesttypeid == selecttype).ToList();
            }
            if (fromdate != null)
            {
                DateTime FromDate = DateTime.Parse(fromdate);
                requests = requests.Where(x => x.Createddate >= FromDate).ToList();
            }
            if (todate != null)
            {
                DateTime ToDate = DateTime.Parse(todate);
                requests = requests.Where(x => x.Createddate <= ToDate).ToList();
            }
            if (providername != null)
            {
                providername = providername.Replace(" ", ""); providername = providername.ToLower();
                requests = requests.Where(x => x.Physician != null).ToList();
                requests = requests.Where(x => (x.Physician.Firstname + x.Physician.Lastname).ToLower().Contains(providername)).ToList();
            }
            if (emailid != null)
            {
                emailid = emailid.Replace(" ", ""); emailid = emailid.ToLower();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Email.ToLower().Contains(emailid)).ToList();
            }
            if (mobile != null)
            {
                mobile = mobile.Replace(" ", ""); mobile = mobile.ToLower();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Phonenumber != null).ToList();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Phonenumber.Contains(mobile)).ToList();
            }


            if (order)
            {
                requests = requests
    .OrderBy(x => x.Requeststatuslogs.Any(z => z.Status == 9) ? x.Requeststatuslogs.Where(z => z.Status == 9).Min(log => log.Createddate) : DateTime.MinValue)
    .ToList();

            }
            else
            {
                requests = requests
    .OrderByDescending(x => x.Requeststatuslogs.Any(z => z.Status == 9) ? x.Requeststatuslogs.Where(z => z.Status == 9).Min(log => log.Createddate) : DateTime.MinValue)
    .ToList();

            }
            requests = requests.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();

            foreach (var item in requests)
            {
                SearchRecodsViewModel search = new SearchRecodsViewModel();
                search.Requestid = item.Requestid;
                search.PatientName = item.Requestclients.FirstOrDefault().Firstname + " " + item.Requestclients.FirstOrDefault().Lastname;
                //search.Requestor = item.Firstname + " " + item.Lastname;
                search.Requestor = item.Requesttype.Name;
                search.DateofService = item.Createddate;
                var lastLog = item.Requeststatuslogs.Where(x => x.Status == 9).LastOrDefault();
                search.CloseCaseDate = lastLog != null ? lastLog.Createddate : null;
                search.Email = item.Requestclients.FirstOrDefault().Email;
                search.Mobile = item.Requestclients.FirstOrDefault().Phonenumber;
                search.Address = item.Requestclients.FirstOrDefault().Address;
                search.Zip = item.Requestclients.FirstOrDefault().Zipcode;
                if (item.Status == 1) search.Status = "New";
                if (item.Status == 2) search.Status = "Pending";
                if (item.Status == 4 || item.Status == 5) search.Status = "Active";
                if (item.Status == 6) search.Status = "Conclude";
                if (item.Status == 3 || item.Status == 7 || item.Status == 8) search.Status = "ToClose";
                if (item.Status == 9) search.Status = "Unpaid";
                if (item.Physician != null)
                {
                    search.Phyicianname = item.Physician.Firstname + " " + item.Physician.Lastname;
                }
                else
                {
                    search.Phyicianname = "-";
                }
                var PhysicianNote = item.Requeststatuslogs.Where(x => x.Physicianid != null).LastOrDefault();
                search.PhysicianNote = PhysicianNote != null ? PhysicianNote.Notes : "_";
                var notelog = item.Requeststatuslogs.Where(x => x.Physicianid != null).LastOrDefault();
                search.CancelledByPhyNote = notelog != null ? notelog.Notes : "-";
                var AdminNote = item.Requeststatuslogs.Where(x => x.Adminid != null).LastOrDefault();
                search.AdminNote = AdminNote != null ? AdminNote.Notes : "_";
                search.PatientNote = item.Requestclients.Last().Notes;
                models.Add(search);
            }
            return PartialView("_SearchRecords", models);
        }

        [Area("AdminArea")]
        [HttpPost]
        public int SearchRecordsCount(string selectstatus, string patientname, int selecttype, string fromdate, string todate, string providername, string emailid, string mobile)
        {
            List<SearchRecodsViewModel> models = new List<SearchRecodsViewModel>();
            var requests = _request.GetRequestsbyState(selectstatus);

            if (patientname != null)
            {
                patientname = patientname.Replace(" ", ""); patientname = patientname.ToLower();
                requests = requests.Where(x => (x.Requestclients.FirstOrDefault().Firstname + x.Requestclients.FirstOrDefault().Lastname).ToLower().Contains(patientname)).ToList();
            }
            if (selecttype != 0)
            {
                requests = requests.Where(x => x.Requesttypeid == selecttype).ToList();
            }
            if (fromdate != null)
            {
                DateTime FromDate = DateTime.Parse(fromdate);
                requests = requests.Where(x => x.Createddate >= FromDate).ToList();
            }
            if (todate != null)
            {
                DateTime ToDate = DateTime.Parse(todate);
                requests = requests.Where(x => x.Createddate <= ToDate).ToList();
            }
            if (providername != null)
            {
                providername = providername.Replace(" ", ""); providername = providername.ToLower();
                requests = requests.Where(x => x.Physician != null).ToList();
                requests = requests.Where(x => (x.Physician.Firstname + x.Physician.Lastname).ToLower().Contains(providername)).ToList();
            }
            if (emailid != null)
            {
                emailid = emailid.Replace(" ", ""); emailid = emailid.ToLower();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Email.ToLower().Contains(emailid)).ToList();
            }
            if (mobile != null)
            {
                mobile = mobile.Replace(" ", ""); mobile = mobile.ToLower();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Phonenumber != null).ToList();
                requests = requests.Where(x => x.Requestclients.FirstOrDefault().Phonenumber.Contains(mobile)).ToList();
            }

            return requests.Count();
        }
    }
}
