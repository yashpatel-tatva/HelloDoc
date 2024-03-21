using DataAccess.Repository;
using DataAccess.ServiceRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers.DataController
{
    [AuthorizationRepository("Admin")]
    public class AccessTabController : Controller
    {
        //private readonly RolemenuRepositorty _rolemenu;
        //private readonly RoleRepository _role;
        //private readonly MenuRepository _menu;
        //public AccessTabController(RoleRepository roleRepository , MenuRepository menuRepository , RolemenuRepositorty rolemenuRepositorty) {
        //    _menu = menuRepository;
        //    _role = roleRepository;
        //    _rolemenu = rolemenuRepositorty;
        //}

        [Area("AdminArea")]
        public IActionResult AccessPage()
        {

            return View();
        }
    }
}
