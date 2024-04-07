using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AdminRecordsTab : Controller
    {
        private readonly HelloDocDbContext _db;

        public AdminRecordsTab(HelloDocDbContext helloDocDbContext)
        {
            _db = helloDocDbContext;
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
        public IActionResult EmailLogsdata(int role , string rname , string email , string createddate , string senddate)
        {
            rname = rname.Replace("ya", "");
            email = email.Replace("ya", "");
            createddate = createddate.Replace("ya", "");
            senddate = senddate.Replace("ya", "");

            List<EmailSMSLogsViewModel> model = new List<EmailSMSLogsViewModel>();
            var emaillogs = _db.Emaillogs.ToList();
            if(role != 0)
            {
                emaillogs = emaillogs.Where(x => x.Roleid == role).ToList();
            }
            if(email != null)
            {
                emaillogs = emaillogs.Where(x => x.Emailid.Contains(email)).ToList();
            }
            //if(createddate != null)
            //{
            //    emaillogs = emaillogs.Where(x => x.Createdate == createddate).ToList();
            //}
            //if(senddate != null)
            //{
            //    emaillogs = emaillogs.Where(x => x.Sentdate == senddate).ToList();
            //}
            foreach(var log in emaillogs)
            {
                EmailSMSLogsViewModel em = new EmailSMSLogsViewModel();
                if(log.Requestid != null)
                {
                    em.Recipient = _db.Requestclients.FirstOrDefault(x => x.Requestid == log.Requestid).Firstname +
                        " " + _db.Requestclients.FirstOrDefault(x => x.Requestid == log.Requestid).Lastname;
                }
                else
                {
                    em.Recipient = log.Emailid.Split('@')[0];
                }
                em.Action = log.Subjectname;
                em.RoleName = _db.Aspnetroles.FirstOrDefault(x => x.Id == role.ToString()).Name;
                em.Email = log.Emailid;
                em.CreatedDate = log.Createdate;
                em.SentDate = (DateTime)log.Sentdate;
                em.Sent = log.Isemailsent[0];
                em.Senttries = (int)log.Senttries;
                if(log.Confirmationnumber != null)
                {
                    em.ConfirmationNo = log.Confirmationnumber;
                }
                model.Add(em);
            }

            return PartialView("_EmailLogs" , model);
        }


        [Area("AdminArea")]
        public IActionResult SmsLogs()
        {
            return View();
        }
        [Area("AdminArea")]
        public IActionResult BlockHistory()
        {
            return View();
        }

    }
}
