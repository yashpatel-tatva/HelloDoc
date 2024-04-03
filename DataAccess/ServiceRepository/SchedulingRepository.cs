using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using HelloDoc;
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
        private readonly HelloDocDbContext _db;

        public SchedulingRepository(IPhysicianRepository physician, IShiftDetailRepository shiftDetail, IShiftRepository shift , HelloDocDbContext helloDocDbContext)
        {
            _physician = physician;
            _shiftDetail = shiftDetail;
            _shift = shift;
            _db = helloDocDbContext;
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

        public List<ShiftData>ShifsOfDate(DateTime datetoshow, int region, int status, int next)
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
            shiftdetail = shiftdetail.OrderBy(x => x.Starttime).ToList();
            foreach (var item in shiftdetail)
            {
                shiftDatas.Add(new ShiftData
                {
                    ShiftId = item.Shiftdetailid,
                    Location = item.Regionid.ToString(),
                    Shiftdate = item.Shiftdate,
                    Description = "Hii",
                    Regionname = _db.Regions.FirstOrDefault(x=>x.Regionid==item.Regionid).Name,
                    StartTime = item.Starttime,
                    EndTime = item.Endtime,
                    Physicianid = _shift.GetFirstOrDefault(x => x.Shiftid == item.Shiftid).Physicianid,
                    Status = _shiftDetail.GetFirstOrDefault(x => x.Shiftdetailid == item.Shiftdetailid).Status,
                });
            }
            if(next != 0)
            {
                shiftDatas = shiftDatas.Skip((next-1)*10).Take(10).ToList();
            }
            return shiftDatas;
        }

        public List<ShiftData> ShifsOfWeek(DateTime datetoshow, int region, int status, int next)
        {
            int daysFromSunday = (int)datetoshow.DayOfWeek;
            int daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)datetoshow.DayOfWeek + 7) % 7;
            DateTime sunday = datetoshow.AddDays(-daysFromSunday);
            DateTime saturday = datetoshow.AddDays(daysUntilSaturday);
            List<ShiftData> shifts = new List<ShiftData>();
            for (DateTime day = sunday; day <= saturday; day = day.AddDays(1))
            {
                List<ShiftData> shiftDatas = ShifsOfDate(day, region, status, 0);
                foreach (var s in shiftDatas)
                {
                    shifts.Add(s);
                }
            }
            shifts =shifts.OrderBy(x=>x.StartTime).ToList();
            if (next != 0)
            {
                var skips = (next - 1) * 10;
                shifts = shifts.Skip(skips).Take(10).ToList();
            }
            return shifts;
        }

        public List<ShiftData> ShifsOfMonth(DateTime datetoshow, int region, int status, int next)
        {
            DateTime firstDayOfMonth = new DateTime(datetoshow.Year, datetoshow.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            List<ShiftData> shifts = new List<ShiftData>();
            for (DateTime day = firstDayOfMonth; day <= lastDayOfMonth; day = day.AddDays(1))
            {
                List<ShiftData> shiftDatas = ShifsOfDate(day, region, status, 0);
                foreach (var s in shiftDatas)
                {
                    shifts.Add(s);
                }
            }
            shifts = shifts.OrderBy(x=>x.StartTime).ToList();   
            if (next != 0)
            {
                var skips = (next - 1) * 10;
                shifts = shifts.Skip(skips).Take(10).ToList();
            }
            return shifts;
        }
    }
}
