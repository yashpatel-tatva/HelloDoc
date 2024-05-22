using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.AdminSideViewModels
{
    public class GroupChatViewModel
    {
        public int? Groupid { get; set; }
        public string? Adminasp { get; set; }
        public string? Physicainasp { get; set; }
        public string? Userasp { get; set; }
        public DateTime? Senttime { get; set; }
        public string? Msg { get; set; }
        public int? Sender { get; set; }
        public int Msgid { get; set; }
        public string fromname { get; set; }

    }
}
