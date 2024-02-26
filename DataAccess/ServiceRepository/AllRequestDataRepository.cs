using DataAccess.Repository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository
{
    public class AllRequestDataRepository : IAllRequestDataRepository
    {
        private readonly HelloDocDbContext _db;
        private readonly IHttpContextAccessor _session;

        public AllRequestDataRepository(HelloDocDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _db = dbContext;
            _session = httpContextAccessor;
        }

        public List<AllRequestDataViewModel> Status(int status)
        {
            List<AllRequestDataViewModel> allRequestDataViewModels = new List<AllRequestDataViewModel>();
            var requests = _db.Requests.Include(r => r.User).Include(r => r.Requestclients).Include(r => r.Physician).ToList().Where(r => r.Status == status);
            foreach (var item in requests)
            {
                AllRequestDataViewModel model = new AllRequestDataViewModel();
                model.PatientName = item.User.Firstname + " " + item.User.Lastname;
                model.PatientDOB = new DateOnly(Convert.ToInt32(item.User.Intyear), DateTime.ParseExact(item.User.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(item.User.Intdate));
                model.RequestorName = item.Firstname + " " + item.Lastname;
                model.RequestedDate = item.Createddate;
                model.PatientPhone = item.User.Mobile;
                model.RequestorPhone = item.Phonenumber;
                model.Address = item.Requestclients.FirstOrDefault(x => x.Requestid == item.Requestid).Address;
                model.Notes = item.Requestclients.FirstOrDefault(x => x.Requestid == item.Requestid).Notes;
                model.PatientEmail = item.User.Email;
                model.RequestorEmail = item.Email;
                model.RequestType = item.Requesttypeid;
                model.Region = _db.Regions.FirstOrDefault(x => x.Regionid == item.User.Regionid).Name;
                model.RequestId = item.Requestid;

                if (status != 1)
                {
                    model.ProviderEmail = item.Physician.Email;
                    model.PhysicainName = item.Physician.Firstname + " " + item.Physician.Lastname;
                }

                allRequestDataViewModels.Add(model);

            }
            return allRequestDataViewModels;

        }

        RequestDataViewModel IAllRequestDataRepository.GetRequestById(int id)
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
            return model;
        }

        RequestNotesViewModel IAllRequestDataRepository.GetNotesById(int id)
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
                var date = _db.Requeststatuslogs.FirstOrDefault(x => x.Requestid == id).Createddate;
                var afterphysican = _db.Requeststatuslogs.FirstOrDefault(y => y.Requestid == id).Transtophysician.Firstname;
                model.TransferNotes = "Admin transferred case to " + afterphysican + " on" + date;
            }
            return model;
        }

        void IAllRequestDataRepository.SaveAdminNotes(int id, RequestNotesViewModel model)
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