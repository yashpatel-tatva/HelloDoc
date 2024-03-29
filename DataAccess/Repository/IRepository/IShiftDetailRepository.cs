﻿using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IShiftDetailRepository : IRepository<Shiftdetail>
    {
        void DeleteThisShift(int shiftdetailid);
    }
}
