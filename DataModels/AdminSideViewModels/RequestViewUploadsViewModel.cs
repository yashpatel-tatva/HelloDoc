using HelloDoc.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class RequestViewUploadsViewModel
    {
        public int RequestsId { get; set; }
        public int reqfileid { get; set; }
        public string confirmation { get; set; }
        public string patientname { get; set; }
        public List<IFormFile> Upload { get; set; }
        public List<Requestwisefile> requestwisefiles { get; set; }
    }
}
