using DataAccess.Repository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
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

        public AllRequestDataRepository(HelloDocDbContext dbContext)
        {
            _db = dbContext;
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