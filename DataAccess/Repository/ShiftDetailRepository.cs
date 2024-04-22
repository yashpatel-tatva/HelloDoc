using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DataAccess.Repository
{
    public class ShiftDetailRepository : Repository<Shiftdetail>, IShiftDetailRepository
    {
        private readonly IShiftRepository _shift;
        public ShiftDetailRepository(HelloDocDbContext db, IShiftRepository shift) : base(db)
        {
            _shift = shift;
        }

        public void DeleteThisShift(int shiftdetailid, string modifiedby)
        {
            var shift = GetFirstOrDefault(x => x.Shiftdetailid == shiftdetailid);
            if (shift != null)
            {
                BitArray fortrue = new BitArray(1);
                fortrue[0] = true;
                shift.Isdeleted = fortrue;
                shift.Modifiedby = modifiedby;
                shift.Modifieddate = DateTime.Now;
                _db.Update(shift);
                _db.SaveChanges();
            }
        }

        public bool EditShiftDetail(int shiftdetailid, DateOnly currentDate, DateTime startTimewithdate, DateTime endTimewithdate, string modifiedby)
        {
            var shiftdetail = GetFirstOrDefault(x => x.Shiftdetailid == shiftdetailid);
            if (shiftdetail == null)
            {
                return false;
            }

            var physicianid = _shift.GetFirstOrDefault(x => x.Shiftid == shiftdetail.Shiftid)?.Physicianid;
            if (physicianid == null)
            {
                return false;
            }

            var shiftdetailexistdata = _db.Shiftdetails
                .Include(x => x.Shift)
                .Where(x => x.Shift.Physicianid == physicianid && x.Shiftdate == currentDate).AsEnumerable().Where(x => x.Isdeleted[0] == false)
                .ToList();

            if (shiftdetailexistdata.Any(s => s.Shiftdetailid != shiftdetailid && !(s.Starttime >= endTimewithdate || s.Endtime <= startTimewithdate)))
            {
                return false;
            }

            shiftdetail.Shiftdate = currentDate;
            shiftdetail.Starttime = startTimewithdate;
            shiftdetail.Endtime = endTimewithdate;
            shiftdetail.Modifiedby = modifiedby;
            shiftdetail.Modifieddate = DateTime.Now;

            _db.Shiftdetails.Update(shiftdetail);
            _db.SaveChanges();

            return true;
        }


        public List<Shiftdetail> getall()
        {
            return _db.Shiftdetails.Include(x => x.Shift).Include(x => x.Shiftdetailregions).ToList();
        }

        public void ReturnThisShift(int shiftdetailid, string modifiedby)
        {
            var shift = GetFirstOrDefault(x => x.Shiftdetailid == shiftdetailid);
            if (shift != null)
            {
                if (shift.Status == 1)
                {
                    shift.Status = 2;
                }
                else if (shift.Status == 2)
                {
                    shift.Status = 1;
                }
                shift.Modifiedby = modifiedby;
                shift.Modifieddate = DateTime.Now;
                _db.Update(shift);
                _db.SaveChanges();
            }
        }
    }
}
