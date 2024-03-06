using DataAccess.Repository.IRepository;
using HelloDoc;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class BlockCaseRepository : Repository<Blockrequest>, IBlockCaseRepository
    {
        private HelloDocDbContext _db;
        private readonly IHttpContextAccessor _httpsession;
        private readonly IAdminRepository _admin;

        public BlockCaseRepository(IHttpContextAccessor httpContextAccessor , IAdminRepository adminRepository, HelloDocDbContext db) : base(db)
        {
            _db = db;
            _httpsession = httpContextAccessor;
            _admin = adminRepository;
        }

        public void BlockRequest(int RequestId, string reason)
        {
            Blockrequest blockrequest = new Blockrequest();
            blockrequest.Requestid = RequestId;
            blockrequest.Reason =  reason;
            blockrequest.Createddate = DateTime.Now;
            blockrequest.Email = _db.Requests.FirstOrDefault(x => x.Requestid == RequestId).Email;
            blockrequest.Phonenumber = _db.Requests.FirstOrDefault(x => x.Requestid == RequestId).Phonenumber;
            if(blockrequest.Phonenumber == null)
            {
                var admin = _admin.GetSessionAdminId();
                var phone = _admin.GetFirstOrDefault(x => x.Adminid == admin).Mobile;
                blockrequest.Phonenumber = phone;
            }
            _db.Blockrequests.Add(blockrequest);

            var request = _db.Requests.FirstOrDefault(x => x.Requestid == RequestId);
            request.Status = 211;
            _db.Requests.Update(request);
            _db.SaveChanges();
        }
    }
}
