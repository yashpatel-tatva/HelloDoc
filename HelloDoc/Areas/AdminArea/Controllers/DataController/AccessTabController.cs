using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using Microsoft.AspNetCore.Mvc;

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
            var Rolename = _role.GetFirstOrDefault(x => x.Roleid ==  roleid).Name;
            _role.DeleteThisRole(roleid);
            TempData["Message"] = Rolename + " Role Created";
            return RedirectToAction("AdminTabsLayout", "Home");
        }
    }
}
