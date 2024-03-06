﻿using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IBlockCaseRepository : IRepository<Blockrequest>
    {
        void BlockRequest(int Requestid , string reason);
    }
}
