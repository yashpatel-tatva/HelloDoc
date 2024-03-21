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
        public AccessTabController(IRoleMenuRepository roleMenuRepository, IRoleRepository role, IMenuRepository menu)
        {
            _rolemenu = roleMenuRepository;
            _role = role;
            _menu = menu;
        }

        [Area("AdminArea")]
        public IActionResult AccessPage()
        {
            return View();
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
    }
}
