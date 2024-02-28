using DataAccess.Repository.IRepository;
using HelloDoc.DataContext;
using HelloDoc.DataModels;
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
        public BlockCaseRepository(IHttpContextAccessor httpContextAccessor, HelloDocDbContext db) : base(db)
        {
            _db = db;
            _httpsession = httpContextAccessor;
        }

        public void BlockRequest(int RequestId, string reason)
        {
            Blockrequest blockrequest = new Blockrequest();
            blockrequest.Requestid = RequestId;
            blockrequest.Reason =  reason;
            blockrequest.Createddate = DateTime.Now;
            blockrequest.Email = _db.Requests.FirstOrDefault(x => x.Requestid == RequestId).Email;
            blockrequest.Phonenumber = _db.Requests.FirstOrDefault(x => x.Requestid == RequestId).Phonenumber;

            _db.Blockrequests.Add(blockrequest);

            var request = _db.Requests.FirstOrDefault(x => x.Requestid == RequestId);
            request.Status = 211;
            _db.Requests.Update(request);
            _db.SaveChanges();
        }
    }
}
