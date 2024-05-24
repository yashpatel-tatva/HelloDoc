using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloDoc.Areas.Hubs
{
    public class HomeController : Controller
    {
        private readonly IAspNetUserRepository _aspnetuser;
        private readonly IAspNetUserRolesRepository _aspnetuserroles;
        private readonly IAdminRepository _admin;
        private readonly IPhysicianRepository _physician;
        private readonly IUserRepository _user;
        private readonly IChatHistoryRepository _chatHistory;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestStatusLogRepository _requestStatusLogRepository;
        private readonly IGroupchatRepository _groupchatRepository;

        public HomeController(IAspNetUserRepository aspnetuser, IAspNetUserRolesRepository aspNetUserRolesRepository, IAdminRepository adminRepository, IPhysicianRepository physicianRepository, IUserRepository userRepository, IChatHistoryRepository chatHistoryRepository, IRequestRepository requestRepository, IRequestStatusLogRepository requestStatusLogRepository, IGroupchatRepository groupchatRepository)
        {
            _aspnetuser = aspnetuser;
            _aspnetuserroles = aspNetUserRolesRepository;
            _admin = adminRepository;
            _physician = physicianRepository;
            _user = userRepository;
            _chatHistory = chatHistoryRepository;
            _requestRepository = requestRepository;
            _requestStatusLogRepository = requestStatusLogRepository;
            _groupchatRepository = groupchatRepository;
        }
        // GET: /<controller>/
        [Area("Hubs")]
        public IActionResult Index()
        {

            return View();
        }

        [Area("Hubs")]
        [HttpPost]
        public IActionResult OpenGroupChatBox(int groupid)
        {
            var statuslog = _requestStatusLogRepository.GetAll()
                .Where(x => x.Requestid == groupid)
               .Where(x => x.Transtophysicianid != null)
               .GroupBy(x => x.Requestid)
               .Select(g => g.OrderByDescending(x => x.Createddate).First())
               .ToList();
            var requests = _requestRepository.GetAll().ToList();
            var name = "";
            var adminid = statuslog.FirstOrDefault(x => x.Requestid == groupid).Adminid;
            var providerid = statuslog.FirstOrDefault(x => x.Requestid == groupid).Transtophysicianid;
            var userid = requests.FirstOrDefault(x => x.Requestid == groupid).Userid;
            var adminname = _admin.GetFirstOrDefault(x => x.Adminid == adminid).Firstname;
            var providername = _physician.GetFirstOrDefault(x => x.Physicianid == providerid).Firstname;
            var username = _user.GetFirstOrDefault(x => x.Userid == userid).Firstname;
            name = adminname + "(Admin) ," + providername + "(Provider) ," + username + "(Patient) ";
            ChatBoxViewModel model = new ChatBoxViewModel();
            model.sendtoaspid = groupid.ToString();
            model.sendtoname = name;
            model.isgroup = true;
            model.photo = "/res/groupphoto.png";
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
            var roleClaimrole = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role").Value;
            var aspid = roleClaim.Value;
            model.role = int.Parse(_aspnetuserroles.GetFirstOrDefault(x => x.Userid == aspid).Roleid);
            model.thisaspid = aspid;
            if (_admin.GetSessionAdminId() != -1)
            {
                if (_admin.GetFirstOrDefault(x => x.Adminid == adminid).Aspnetuserid != aspid)
                {
                    return BadRequest("Can't Access");
                }
            }
            return PartialView("_ChatBox", model);
        }


        [Area("Hubs")]
        [HttpPost]
        public IActionResult OpenChatBox(string sendtoaspid)
        {
            if (sendtoaspid == "AdminChatGroup")
            {
                var admin = _admin.GetAll().ToList();
                return PartialView("_selectAdminToTalk", admin);
            }
            var asp = _aspnetuser.GetFirstOrDefault(x => x.Id == sendtoaspid);
            var role = _aspnetuserroles.GetRoleFromId(asp.Id);
            var name = "";
            var photo = "";
            if (role == 1)
            {
                name = _admin.GetFirstOrDefault(x => x.Aspnetuserid == asp.Id).Firstname + " " + _admin.GetFirstOrDefault(x => x.Aspnetuserid == asp.Id).Lastname;
            }
            if (role == 2)
            {
                name = _physician.GetFirstOrDefault(x => x.Aspnetuserid == asp.Id).Firstname + " " + _physician.GetFirstOrDefault(x => x.Aspnetuserid == asp.Id).Lastname;
                photo = _physician.GetFirstOrDefault(x => x.Aspnetuserid == asp.Id).Photo;
            }
            if (role == 3)
            {
                name = _user.GetFirstOrDefault(x => x.Aspnetuserid == asp.Id).Firstname + " " + _user.GetFirstOrDefault(x => x.Aspnetuserid == asp.Id).Lastname;
            }
            ChatBoxViewModel model = new ChatBoxViewModel();
            model.sendtoaspid = sendtoaspid;
            model.sendtoname = name;
            model.photo = photo;
            model.isgroup = false;

            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
            var aspid = roleClaim.Value;
            model.thisaspid = aspid;
            return PartialView("_ChatBox", model);
        }


        [Area("Hubs")]
        [HttpPost]
        public List<Chathistory> ChatHistoryOfthisTwo(string opp)
        {
            var jwtservice = HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
            var aspid = roleClaim.Value;
            var history = _chatHistory.HistoryOfthisTwo(aspid, opp).OrderBy(x => x.Senttime).ToList();
            return history;
        }

        [Area("Hubs")]
        [HttpPost]
        public List<GroupChatViewModel> ChatHistoryOfthisgroup(string opp)
        {
            var history = _groupchatRepository.HistoryOfthisgroup(opp).OrderBy(x => x.Senttime).ToList();
            List<GroupChatViewModel> list = new List<GroupChatViewModel>();
            foreach (var h in history)
            {
                GroupChatViewModel groupChatViewModel = new GroupChatViewModel();
                groupChatViewModel.Groupid = h.Groupid;
                groupChatViewModel.Adminasp = h.Adminasp;
                groupChatViewModel.Physicainasp = h.Physicainasp;
                groupChatViewModel.Userasp = h.Userasp;
                groupChatViewModel.Senttime = h.Senttime;
                groupChatViewModel.Msg = h.Msg;
                groupChatViewModel.Sender = h.Sender;
                groupChatViewModel.Msgid = h.Msgid;
                if (h.Sender == 1)
                    groupChatViewModel.fromname = _aspnetuser.GetFirstOrDefault(x => x.Id == h.Adminasp).Username;
                if (h.Sender == 2)
                    groupChatViewModel.fromname = _aspnetuser.GetFirstOrDefault(x => x.Id == h.Physicainasp).Username;
                if (h.Sender == 3)
                    groupChatViewModel.fromname = _aspnetuser.GetFirstOrDefault(x => x.Id == h.Userasp).Username;
                list.Add(groupChatViewModel);
            }

            return list;
        }


    }
}

