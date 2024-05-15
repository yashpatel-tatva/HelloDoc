using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;

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

        public HomeController(IAspNetUserRepository aspnetuser, IAspNetUserRolesRepository aspNetUserRolesRepository, IAdminRepository adminRepository, IPhysicianRepository physicianRepository, IUserRepository userRepository, IChatHistoryRepository chatHistoryRepository)
        {
            _aspnetuser = aspnetuser;
            _aspnetuserroles = aspNetUserRolesRepository;
            _admin = adminRepository;
            _physician = physicianRepository;
            _user = userRepository;
            _chatHistory = chatHistoryRepository;
        }
        // GET: /<controller>/
        [Area("Hubs")]
        public IActionResult Index()
        {

            return View();
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
            var history = _chatHistory.HistoryOfthisTwo(aspid , opp).OrderBy(x=>x.Senttime).ToList();
            return history;
        }


    }
}

