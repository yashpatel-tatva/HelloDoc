using HelloDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class VendorsListViewModel
    {
        public List<Healthprofessional> healthprofessionals { get; set; }
        public List<Healthprofessionaltype> healthprofessionaltypes { get; set; }
    }
}
