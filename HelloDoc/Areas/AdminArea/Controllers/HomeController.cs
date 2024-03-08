using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using Microsoft.AspNetCore.Mvc;

namespace HelloDoc.Areas.AdminArea.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdminRepository _admin;

        public HomeController(IAdminRepository admin)
        {
            _admin = admin;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Area("AdminArea")]
        public IActionResult AdminLogin()
        {
            return View();
        }


        [Area("AdminArea")]
        public IActionResult AdminForgetPassword() 
        { 
            return View(); 
        }
        [Area("AdminArea")]
        public IActionResult ExpirePopUp() 
        { 
            return View(); 
        }


        [Area("AdminArea")]
        [AuthorizationRepository("Admin")]
        public IActionResult AdminTabsLayout()
        {
            var aspnetid = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().ReadJwtToken(Request.Cookies["jwt"]).Claims.FirstOrDefault(x => x.Type == "AspNetId").Value;

            if (aspnetid == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }

            _admin.SetSession(_admin.GetFirstOrDefault(x => x.Aspnetuserid == aspnetid));

            return View();
        }


    }
}
