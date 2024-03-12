using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            )
        {
            _db = helloDocDbContext;
            _blockcase = blockCaseRepository;
            _admin = adminRepository;
            _request = requestRepository;
            _requeststatus = requestStatusLogRepository;
        }

        public void AssignCase(int requestId, int physicianId, int assignby, string description)
        {
            var request = _request.GetFirstOrDefault(x => x.Requestid == requestId);
            request.Status = 2;
            request.Physicianid = physicianId;
            _request.Update(request);

            Requeststatuslog requeststatuslog = new Requeststatuslog();
            requeststatuslog.Requestid = requestId;
            requeststatuslog.Status = 2;
            requeststatuslog.Transtophysicianid = physicianId;
            requeststatuslog.Createddate = DateTime.Now;
            requeststatuslog.Notes = description;
            requeststatuslog.Adminid = assignby;

            _requeststatus.Add(requeststatuslog);

            _db.SaveChanges();

        }








        public void BlockCase(int requestId, string description)
        {
            _blockcase.BlockRequest(requestId, description);

        }

        public void CancelCase(int requestid, int casetag, string note)
        {
            var id = requestid;
            var reason = note;

            var request = _request.GetFirstOrDefault(x => x.Requestid == id);
            if (request.Casetag != null) { request.Casetag = request.Casetag + " , " + casetag; }
            else { request.Casetag = casetag.ToString(); }
            request.Status = 3;
            _request.Update(request);
            _request.Save();
            Requeststatuslog requeststatuslog = new Requeststatuslog();
            requeststatuslog.Requestid = id;
            if (_admin.GetSessionAdminId() != null)
            {
                requeststatuslog.Adminid = _admin.GetSessionAdminId();
            }
            requeststatuslog.Status = 3;
            requeststatuslog.Notes = reason;
            requeststatuslog.Createddate = DateTime.Now;
            _requeststatus.Add(requeststatuslog);
            _requeststatus.Save();
        }

        public void ClearCase(int requestid)
        {
            var request = _request.GetFirstOrDefault(x => x.Requestid == requestid);
            request.Status = 10;
            _request.Update(request);
            _request.Save();
            Requeststatuslog requeststatuslog = new Requeststatuslog();
            requeststatuslog.Requestid = requestid;
            requeststatuslog.Adminid = _admin.GetSessionAdminId();
            requeststatuslog.Status = 10;
            requeststatuslog.Createddate = DateTime.Now;
            _requeststatus.Add(requeststatuslog);
            _requeststatus.Save();
        }

        public void CloseCase(int requestid)
        {
            var request = _request.GetFirstOrDefault(x => x.Requestid == requestid);
            request.Status = 9;
            _request.Update(request);
            _request.Save();
            Requeststatuslog requeststatuslog = new Requeststatuslog();
            requeststatuslog.Requestid = requestid;
            requeststatuslog.Adminid = _admin.GetSessionAdminId();
            requeststatuslog.Status = 9;
            requeststatuslog.Createddate = DateTime.Now;
            _requeststatus.Add(requeststatuslog);
            _requeststatus.Save();
        }
    }
}
