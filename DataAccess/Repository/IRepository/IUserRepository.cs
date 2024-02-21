﻿using HelloDoc.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {

        void Update(User user);
        void Save();
    }
}
