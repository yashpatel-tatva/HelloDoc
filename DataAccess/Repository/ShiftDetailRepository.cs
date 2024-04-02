using DataAccess.Repository.IRepository;
using HelloDoc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ShiftDetailRepository : Repository<Shiftdetail>, IShiftDetailRepository
    {
        private readonly IShiftRepository _shift;
        public ShiftDetailRepository(HelloDocDbContext db, IShiftRepository shift) : base(db)
        {
            _shift = shift;
        }

        public void DeleteThisShift(int shiftdetailid)
        {
            var shift = GetFirstOrDefault(x => x.Shiftdetailid == shiftdetailid);
            if (shift != null)
            {
                BitArray fortrue = new BitArray(1);
                fortrue[0] = true;
                shift.Isdeleted = fortrue;
                _db.Update(shift);
                _db.SaveChanges();
            }
        }

        public bool EditShiftDetail(int shiftdetailid, DateOnly currentDate, DateTime startTimewithdate, DateTime endTimewithdate, string modifiedby)
        {
            var IsValid = false;
            var shiftdetail = GetFirstOrDefault(x => x.Shiftdetailid == shiftdetailid);
            var physicianid = _shift.GetFirstOrDefault(x => x.Shiftid == shiftdetail.Shiftid).Physicianid;
            var shiftdetailexistdata = _db.Shiftdetails.Include(x => x.Shift).Where(x => x.Shift.Physicianid == physicianid).Where(x => x.Shiftdate == currentDate);
            if (currentDate == shiftdetail.Shiftdate)
            {
                IsValid = true;
            }
            else
            {
                if (shiftdetailexistdata.Count() != 0)
                {
                    foreach (var s in shiftdetailexistdata)
                    {
                        if ((s.Starttime > startTimewithdate && s.Starttime > endTimewithdate) || (s.Endtime < startTimewithdate && s.Endtime < endTimewithdate))
                        {
                            IsValid = true;
                        }
                        else
                        {
                            currentDate = currentDate.AddDays(1);
                            continue;
                        }
                    }
                }
                else
                {
                    IsValid = true;
                }
            }
            if (IsValid)
            {
                shiftdetail.Shiftdate = currentDate;
                shiftdetail.Starttime = startTimewithdate;
                shiftdetail.Endtime = endTimewithdate;
                shiftdetail.Modifiedby = modifiedby;
                shiftdetail.Modifieddate = DateTime.Now;
                _db.Shiftdetails.Update(shiftdetail);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReturnThisShift(int shiftdetailid)
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
                _db.Update(shift);
                _db.SaveChanges();
            }
        }
    }
}
