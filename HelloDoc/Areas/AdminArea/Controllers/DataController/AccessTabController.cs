using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using DataModels.AdminSideViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AccessTabController : Controller
    {
        private readonly IRoleMenuRepository _rolemenu;
        private readonly IRoleRepository _role;
        private readonly IMenuRepository _menu;
        private readonly IAdminRepository _admin;
        private readonly HelloDocDbContext _db;
        public AccessTabController(IRoleMenuRepository roleMenuRepository, IRoleRepository role, IMenuRepository menu, IAdminRepository adminRepository, HelloDocDbContext helloDocDbContext)
        {
            _rolemenu = roleMenuRepository;
            _role = role;
            _menu = menu;
            _admin = adminRepository;
            _db = helloDocDbContext;
        }

        [Area("AdminArea")]
        public IActionResult AccessPage()
        {
            List<Role> roles = new List<Role>();
            roles = _role.GetAllRolesToSelect();
            return View(roles);

        }
        [Area("AdminArea")]
        public IActionResult CreateRolePage()
        {
            return View();
        }

        [Area("AdminArea")]
        public IActionResult UserAcess()
        {
            return View();
        }
        [Area("AdminArea")]
        [HttpPost]
        public List<Menu> GetMenus(int accounttype)
        {
            List<Menu> menuList = new List<Menu>();
            menuList = _menu.GetAll().ToList();
            if (accounttype != 0)
            {
                menuList = menuList.Where(x => x.Accounttype == accounttype).ToList();
            }
            return menuList;
        }
        [Area("AdminArea")]
        [HttpGet]
        public List<Menu> GetSelectedMenusByRoleId(int accounttype, int roleid)
        {
            var result = _role.GetMenusByRole(roleid).ToList();
            if (accounttype != 0)
            {
                result = result.Where(x => x.Accounttype == accounttype).ToList();
            }
            return result;
        }
        [Area("AdminArea")]
        [HttpGet]
        public List<Menu> GetRemainingMenusByRole(int accounttype, int roleid)
        {
            var result = _role.GetRemainingMenusByRole(roleid).ToList();
            if (accounttype != 0)
            {
                result = result.Where(x => x.Accounttype == accounttype).ToList();
            }
            return result;
        }

        [Area("AdminArea")]
        [HttpPost]
        public bool IsThisRoleExist(string Role_Name)
        {
            var name = _db.Roles.FirstOrDefault(x => x.Name == Role_Name);
            if (name == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult AddThisRole(string Rolename, List<int> menuitems, int accounttype)
        {
            _role.AddThisRole(Rolename, accounttype, menuitems, _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid);
            TempData["Message"] = Rolename + " Role Created";
            return RedirectToAction("AdminTabsLayout", "Home");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult DeleteThisRole(int roleid)
        {
            var Rolename = _role.GetFirstOrDefault(x => x.Roleid == roleid).Name;
            _role.DeleteThisRole(roleid);
            TempData["Message"] = Rolename + " Role Created";
            return RedirectToAction("AdminTabsLayout", "Home");
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditRolePage(int roleid)
        {
            var rolename = _role.GetFirstOrDefault(x => x.Roleid == roleid).Name;
            var accounttype = _role.GetFirstOrDefault(x => x.Roleid == roleid).Accounttype;
            return View(new { roleid, rolename, accounttype });
        }
        [Area("AdminArea")]
        [HttpPost]
        public IActionResult EditThisRole(int Roleid, List<int> menuitems, int accounttype)
        {
            _role.EditThisRole(Roleid, accounttype, menuitems, _admin.GetFirstOrDefault(x => x.Adminid == _admin.GetSessionAdminId()).Aspnetuserid);
            var Rolename = _role.GetFirstOrDefault(x => x.Roleid == Roleid).Name;
            TempData["Message"] = Rolename + " Role Edited";
            return RedirectToAction("AdminTabsLayout", "Home");
        }


        [Area("AdminArea")]
        [HttpPost]
        public int UserCountbyFilter(int role)
        {
            var userid = _db.Aspnetusers.Count();
            if(role != 0)
            {
                userid = _db.Aspnetuserroles.Where(x => x.Roleid.Contains(role.ToString())).Count();
            }
            return userid;
        }

        [Area("AdminArea")]
        [HttpPost]
        public IActionResult AllUsersDataFilter(int role, int currentpage, int pagesize)
        {
            var usersid = _db.Aspnetuserroles.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            if (role != 0)
            {
                usersid = _db.Aspnetuserroles.Where(x => x.Roleid.Contains(role.ToString())).Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            }
            var users = _db.Aspnetusers.Include(x => x.Aspnetuserroles).Where(x => usersid.Select(x => x.Userid).Contains(x.Id)).ToList();
            List<UserAccessViewModel> list = new List<UserAccessViewModel>();
            foreach (var user in users)
            {
                UserAccessViewModel model = new UserAccessViewModel();
                model.AspnetId = user.Id;
                model.Accounttype = user.Aspnetuserroles.FirstOrDefault().Roleid;
                if (model.Accounttype == "1")
                {
                    var thisuser = _admin.GetFirstOrDefault(x => x.Aspnetuserid == user.Id);
                    model.UserId = thisuser.Adminid;
                    model.UserName = thisuser.Firstname + " " + thisuser.Lastname;
                    model.Phone = thisuser.Mobile;
                    if (thisuser.Status != null)
                    {
                        model.Status = (int)thisuser.Status;
                    }
                    model.Openrequest = _db.Requests.Where(x => x.Status != 9).AsEnumerable().Where(x => x.Isdeleted == null || x.Isdeleted[0] == false).Count();
                }
                if (model.Accounttype == "2")
                {
                    var thisuser = _db.Physicians.FirstOrDefault(x => x.Aspnetuserid == user.Id);
                    model.UserId = thisuser.Physicianid;
                    model.UserName = thisuser.Firstname + " " + thisuser.Lastname;
                    model.Phone = thisuser.Mobile;
                    if (thisuser.Status != null)
                    {
                        model.Status = (int)thisuser.Status;
                    }
                    model.Openrequest = _db.Requests.Where(x => x.Status != 9 && x.Physicianid == thisuser.Physicianid).AsEnumerable().Where(x => x.Isdeleted == null || x.Isdeleted[0] == false).Count();
                }
                if (model.Accounttype == "3")
                {
                    var thisuser = _db.Users.FirstOrDefault(x => x.Aspnetuserid == user.Id);
                    model.UserId = thisuser.Userid;
                    model.UserName = thisuser.Firstname + " " + thisuser.Lastname;
                    model.Phone = thisuser.Mobile;
                    if (thisuser.Status != null)
                    {
                        model.Status = (int)thisuser.Status;
                    }
                    model.Openrequest = _db.Requests.Where(x => x.Status != 9 && x.Userid == thisuser.Userid).AsEnumerable().Where(x => x.Isdeleted == null || x.Isdeleted[0] == false).Count();
                }
                list.Add(model);
            }
            return PartialView("_UserAccess", list);
        }

    }
}
