using DataAccess.Repository.IRepository;
using HelloDoc;
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
        public ShiftDetailRepository(HelloDocDbContext db) : base(db)
        {
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
