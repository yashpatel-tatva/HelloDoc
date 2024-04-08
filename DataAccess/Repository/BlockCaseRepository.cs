using DataAccess.Repository.IRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace DataAccess.Repository
{
    public class BlockCaseRepository : Repository<Blockrequest>, IBlockCaseRepository
    {
        private HelloDocDbContext _db;
        private readonly IHttpContextAccessor _httpsession;
        private readonly IAdminRepository _admin;
        private readonly IRequestRepository _request;

        public BlockCaseRepository(IHttpContextAccessor httpContextAccessor, IAdminRepository adminRepository, IRequestRepository request, HelloDocDbContext db) : base(db)
        {
            _db = db;
            _httpsession = httpContextAccessor;
            _admin = adminRepository;
            _request = request;
        }

        public void BlockRequest(int RequestId, string reason)
        {
            Blockrequest blockrequest = new Blockrequest();
            blockrequest.Requestid = RequestId;
            blockrequest.Reason = reason;
            blockrequest.Createddate = DateTime.Now;
            blockrequest.Email = _db.Requests.FirstOrDefault(x => x.Requestid == RequestId).Email;
            blockrequest.Phonenumber = _db.Requests.FirstOrDefault(x => x.Requestid == RequestId).Phonenumber;
            BitArray fortrue = new BitArray(1); fortrue[0] = true;
            if (blockrequest.Phonenumber == null)
            {
                var admin = _admin.GetSessionAdminId();
                var phone = _admin.GetFirstOrDefault(x => x.Adminid == admin).Mobile;
                blockrequest.Phonenumber = phone;
            }
            blockrequest.Isactive = fortrue;
            _db.Blockrequests.Add(blockrequest);

            var request = _db.Requests.FirstOrDefault(x => x.Requestid == RequestId);
            request.Status = 211;
            _db.Requests.Update(request);
            _db.SaveChanges();
        }

        public List<BlockHistoryVIewModel> GetBlockHistory()
        {
            var blockhistory = GetAll().ToList();
            List<BlockHistoryVIewModel> models = new List<BlockHistoryVIewModel>();

            foreach (var item in blockhistory)
            {
                BlockHistoryVIewModel blockHistory = new BlockHistoryVIewModel();
                blockHistory.Blockid = item.Blockrequestid;
                blockHistory.Name = _db.Requestclients.FirstOrDefault(x => x.Requestid == item.Requestid).Firstname + " " + _db.Requestclients.FirstOrDefault(x => x.Requestid == item.Requestid).Lastname;
                blockHistory.Mobile = item.Phonenumber;
                blockHistory.Email = item.Email;
                blockHistory.Notes = item.Reason;
                blockHistory.IsActive = item.Isactive[0];
                blockHistory.CreatedDate = item.Createddate;
                models.Add(blockHistory);
            }

            return models;
        }

        public void UnblockThis(int id)
        {
            var blockrequest = GetFirstOrDefault(x => x.Blockrequestid == id);
            var request = _request.GetFirstOrDefault(x => x.Requestid == blockrequest.Requestid);
            request.Status = 1;
            _db.Requests.Update(request);

            _db.Blockrequests.Remove(blockrequest);
            _db.SaveChanges();


        }
    }
}
