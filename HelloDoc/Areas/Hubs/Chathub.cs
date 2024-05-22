using System;
using DataAccess.ServiceRepository.IServiceRepository;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR;
using DataAccess.Repository.IRepository;
using NuGet.Protocol.Plugins;
using NPOI.SS.Formula.Functions;

namespace HelloDoc.Areas.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAspNetUserRepository _aspnet;
        private readonly IAspNetUserRolesRepository _aspnetRoles;
        private readonly IChatHistoryRepository _chatHistory;
        private readonly IAdminRepository _admin;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestStatusLogRepository _requestStatusLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhysicianRepository _hysicianRepository;
        private readonly IGroupchatRepository _groupchatRepository;
        private readonly HelloDocDbContext _db;
        public ChatHub(IHttpContextAccessor httpContextAccessor, IAspNetUserRepository aspNetUserRepository, IAspNetUserRolesRepository aspNetUserRolesRepository, IChatHistoryRepository chatHistory, IAdminRepository adminRepository, IRequestRepository requestRepository, IRequestStatusLogRepository requestStatusLogRepository, IUserRepository userRepository, IPhysicianRepository hysicianRepository, IGroupchatRepository groupchatRepository, HelloDocDbContext db)
        {
            _contextAccessor = httpContextAccessor;
            _aspnetRoles = aspNetUserRolesRepository;
            _aspnet = aspNetUserRepository;
            _chatHistory = chatHistory;
            _admin = adminRepository;
            _requestRepository = requestRepository;
            _requestStatusLogRepository = requestStatusLogRepository;
            _userRepository = userRepository;
            _hysicianRepository = hysicianRepository;
            _groupchatRepository = groupchatRepository;
            _db = db;
        }
        public async Task SendMessage(string sendid, string msg)
        {
            var jwtservice = _contextAccessor.HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = _contextAccessor.HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
            var aspid = roleClaim.Value;
            var fromname = _aspnet.GetFirstOrDefault(x => x.Id == aspid).Username;
            var fromid = aspid;
            var client = Context.ConnectionId;
            var time = DateTime.Now;

            var msgid = -1;
            if (sendid == "AdminChatGroup")
            {
                foreach (var admin in _admin.GetAll().ToList())
                {
                    Chathistory ch = new Chathistory();
                    ch.Msg = msg;
                    ch.Sender = fromid;
                    ch.Reciever = admin.Aspnetuserid;
                    ch.Isread = false;
                    ch.Senttime = DateTime.Now;
                    _chatHistory.Add(ch);
                    _chatHistory.Save();
                }
            }
            else
            {
                Chathistory ch1 = new Chathistory();
                ch1.Msg = msg;
                ch1.Sender = fromid;
                ch1.Reciever = sendid;
                ch1.Isread = false;
                ch1.Senttime = DateTime.Now;
                _chatHistory.Add(ch1);
                _chatHistory.Save();
                msgid = ch1.Msgid;
            }

            await Clients.Group(sendid).SendAsync("ReceiveMessage", fromname, fromid, msg, time, msgid);
        }

        public async Task SendMessageAsGroup(string sendid, string msg)
        {
            var jwtservice = _contextAccessor.HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = _contextAccessor.HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
            var aspid = roleClaim.Value;
            var fromname = _aspnet.GetFirstOrDefault(x => x.Id == aspid).Username;
            var fromid = aspid;
            var client = Context.ConnectionId;
            var time = DateTime.Now;
            var statuslog = _requestStatusLogRepository.GetAll()
                .Where(x => x.Requestid == int.Parse(sendid))
               .Where(x => x.Transtophysicianid != null)
               .GroupBy(x => x.Requestid)
               .Select(g => g.OrderByDescending(x => x.Createddate).First())
               .ToList();
            var requests = _requestRepository.GetAll().ToList();
            var adminid = statuslog.FirstOrDefault(x => x.Requestid == int.Parse(sendid)).Adminid;
            var providerid = statuslog.FirstOrDefault(x => x.Requestid == int.Parse(sendid)).Transtophysicianid;
            var userid = requests.FirstOrDefault(x => x.Requestid == int.Parse(sendid)).Userid;

            var adminasp = _admin.GetFirstOrDefault(x => x.Adminid == adminid).Aspnetuserid;
            var phyasp = _hysicianRepository.GetFirstOrDefault(x => x.Physicianid == providerid).Aspnetuserid;
            var userasp = _userRepository.GetFirstOrDefault(x => x.Userid == userid).Aspnetuserid;

            Groupchat ch = new Groupchat();
            ch.Groupid = int.Parse(sendid);
            ch.Adminasp = adminasp;
            ch.Physicainasp = phyasp;
            ch.Userasp = userasp;
            ch.Senttime = DateTime.Now;
            ch.Msg = msg;
            ch.Sender = int.Parse(_aspnetRoles.GetFirstOrDefault(x => x.Userid == fromid).Roleid);
            
            _db.Groupchats.Add(ch);
            _db.SaveChanges();

            var msgid = -1;


            await Clients.Group(sendid).SendAsync("ReceiveMessageInGroup", fromname, fromid, msg, time, msgid , sendid);
        }

        public async Task MsgSeen(string fromthis)
        {
            var jwtservice = _contextAccessor.HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = _contextAccessor.HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
            var aspid = roleClaim.Value;
            _chatHistory.MsgSeen(aspid, fromthis);
            await Clients.Group(fromthis).SendAsync("MsgSeen", fromthis, aspid);
        }


        public async Task MsgSent()
        {
            var jwtservice = _contextAccessor.HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = _contextAccessor.HttpContext.Request;
            var token = request.Cookies["jwt"];
            var aspid = "";

            if (token != null)
            {
                jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
                var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
                aspid = roleClaim.Value;
                _chatHistory.MsgSent(aspid);
            }
            await Clients.All.SendAsync("MsgSent", aspid);
        }


        public string GetConnectionID()
        {
            var val = Context.ConnectionId;
            return val;
        }

        public async Task AddToGroup(string groupName)
        {
            var statuslog = _requestStatusLogRepository.GetAll()
                .Where(x => x.Transtophysicianid != null)
                .GroupBy(x => x.Requestid)
                .Select(g => g.OrderByDescending(x => x.Createddate).First())
                .ToList();
            var requests = _requestRepository.GetAll().ToList();
            var jwtservice = _contextAccessor.HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = _contextAccessor.HttpContext.Request;
            var token = request.Cookies["jwt"];
            if (token != null)
            {
                jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
                var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
                if (groupName == null)
                {
                    groupName = roleClaim.Value;
                }
                var role = _aspnetRoles.GetRoleFromId(roleClaim.Value);
                if (role == 1)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "AdminChatGroup");
                    foreach (var status in statuslog.Where(x => x.Adminid == _admin.GetFirstOrDefault(x => x.Aspnetuserid == groupName).Adminid))
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, status.Requestid.ToString());
                    }
                }
                if (role == 2)
                {
                    foreach (var status in statuslog.Where(x => x.Transtophysicianid == _hysicianRepository.GetFirstOrDefault(x => x.Aspnetuserid == groupName).Physicianid))
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, status.Requestid.ToString());
                    }
                }
                if (role == 3)
                {
                    var userid = _userRepository.GetFirstOrDefault(x => x.Aspnetuserid == groupName);
                    foreach (var req in requests)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, req.Requestid.ToString());
                    }
                }
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
            }
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}

