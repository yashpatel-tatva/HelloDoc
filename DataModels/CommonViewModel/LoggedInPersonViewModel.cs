﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.CommonViewModel
{
    public class LoggedInPersonViewModel
    {
        public string AspnetId { get; set; }
        public string UserName { get; set; }

        public string Role { get; set; }
    }
}
