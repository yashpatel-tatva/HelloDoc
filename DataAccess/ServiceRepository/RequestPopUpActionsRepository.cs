using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository
{
    public class RequestPopUpActionsRepository : IRequestPopUpActionsRepository
    {
        private readonly HelloDocDbContext _db;
        private readonly IBlockCaseRepository _blockcase;
        private readonly IAdminRepository _admin;
        private readonly IRequestRepository _request;
        private readonly IRequestStatusLogRepository _requeststatus;

        public RequestPopUpActionsRepository(
            HelloDocDbContext helloDocDbContext,
            IBlockCaseRepository blockCaseRepository,
            IAdminRepository adminRepository,
            IRequestRepository requestRepository,
            IRequestStatusLogRepository requestStatusLogRepository
            ) {
            _db = helloDocDbContext;
            _blockcase = blockCaseRepository;
            _admin = adminRepository;
            _request = requestRepository;
            _requeststatus = requestStatusLogRepository;
        }

        public void AssignCase(int requestId, int physicianId, int assignby , string description)
        {
            var request = _request.GetFirstOrDefault(x=>x.Requestid ==  requestId);
            request.Status = 2;
            request.Physicianid = physicianId;
            _request.Update(request);

            Requeststatuslog requeststatuslog   = new Requeststatuslog();
            requeststatuslog.Requestid = requestId;
            requeststatuslog.Status = 2;
            requeststatuslog.Transtophysicianid = physicianId;
            requeststatuslog.Createddate = DateTime.Now;
            requeststatuslog.Notes = description;
            requeststatuslog.Adminid = assignby;

            _requeststatus.Add(requeststatuslog);

            _db.SaveChanges();

        }








        public void BlockCase(DashpopupsViewModel model)
        {
            var adminid = _admin.GetSessionAdminId();
            var email = _db.Admins.FirstOrDefault(x => x.Adminid == adminid).Email;

            _blockcase.BlockRequest(model.RequestId, model.Blockreason);

        }

        public void CancelCase(DashpopupsViewModel model)
        {
            var id = model.RequestId;
            var casetag = model.CaseTagID;
            var reason = model.Notes;

            var request = _request.GetFirstOrDefault(x => x.Requestid == id);
            if(request.Casetag != null) { request.Casetag = request.Casetag + " , " + casetag;}
            else { request.Casetag = casetag.ToString(); }
            request.Status = 3;
            _request.Update(request);
            _request.Save();
            Requeststatuslog requeststatuslog = new Requeststatuslog();
            requeststatuslog.Requestid = id;
            requeststatuslog.Adminid = _admin.GetSessionAdminId();
            requeststatuslog.Status = 3;
            requeststatuslog.Notes = reason;
            requeststatuslog.Createddate = DateTime.Now;

            _requeststatus.Add(requeststatuslog);
            _request.Save();
        }
    }
}
