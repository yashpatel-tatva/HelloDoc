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
        public ChatHub(IHttpContextAccessor httpContextAccessor, IAspNetUserRepository aspNetUserRepository, IAspNetUserRolesRepository aspNetUserRolesRepository, IChatHistoryRepository chatHistory, IAdminRepository adminRepository)
        {
            _contextAccessor = httpContextAccessor;
            _aspnetRoles = aspNetUserRolesRepository;
            _aspnet = aspNetUserRepository;
            _chatHistory = chatHistory;
            _admin = adminRepository;
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

        public async Task MsgSeen(string fromthis)
        {
            var jwtservice = _contextAccessor.HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = _contextAccessor.HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
            var aspid = roleClaim.Value;
            _chatHistory.MsgSeen(aspid , fromthis);
        }
        
        public async Task MsgSent()
        {
            var jwtservice = _contextAccessor.HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = _contextAccessor.HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
            var aspid = roleClaim.Value;
            _chatHistory.MsgSent(aspid);
        }


        public string GetConnectionID()
        {
            var val = Context.ConnectionId;
            return val;
        }

        public async Task AddToGroup(string groupName)
        {
            var jwtservice = _contextAccessor.HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = _contextAccessor.HttpContext.Request;
            var token = request.Cookies["jwt"];
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
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}

