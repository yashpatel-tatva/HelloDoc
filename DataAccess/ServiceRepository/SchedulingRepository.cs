using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceRepository
{
    public class SchedulingRepository : ISchedulingRepository
    {
        private readonly IPhysicianRepository _physician;
        private readonly IShiftDetailRepository _shiftDetail;
        private readonly IShiftRepository _shift;

        public SchedulingRepository(IPhysicianRepository physician, IShiftDetailRepository shiftDetail, IShiftRepository shift)
        {
            _physician = physician;
            _shiftDetail = shiftDetail;
            _shift = shift;
        }

        public List<PhysicianData> GetPhysicianData()
        {
            List<PhysicianData> physicianDatas = new List<PhysicianData>();
            var phy = _physician.GetAll().Where(x => x.Isdeleted[0] == false);
            foreach (var item in phy)
            {
                physicianDatas.Add(new PhysicianData { Physicianid = item.Physicianid, Physicianname = item.Firstname + " " + item.Lastname, Photo = item.Photo });
            }
            return physicianDatas;
        }

        public List<ShiftData> ShifsOfDate(DateTime datetoshow, int region, int status, int next)
        {
            List<ShiftData> shiftDatas = new List<ShiftData>();
            DateOnly currentday = DateOnly.FromDateTime(datetoshow);
            var shiftdetail = _shiftDetail.GetAll().Where(x => x.Isdeleted[0] == false).Where(x => x.Shiftdate == currentday);
            if (region != 0)
            {
                shiftdetail = shiftdetail.Where(x => x.Regionid == region);
            }
            if (status != 0)
            {
                shiftdetail = shiftdetail.Where(x => x.Status == status);
            }
            foreach (var item in shiftdetail)
            {
                shiftDatas.Add(new ShiftData
                {
                    ShiftId = item.Shiftdetailid,
                    Location = item.Regionid.ToString(),
                    Description = "Hii",
                    StartTime = item.Starttime,
                    EndTime = item.Endtime,
                    Physicianid = _shift.GetFirstOrDefault(x => x.Shiftid == item.Shiftid).Physicianid,
                    Status = _shiftDetail.GetFirstOrDefault(x => x.Shiftdetailid == item.Shiftdetailid).Status,
                });
            }
            return shiftDatas;
        }
    }
}
