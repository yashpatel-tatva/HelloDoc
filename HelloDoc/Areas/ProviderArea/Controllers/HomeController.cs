using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace HelloDoc.Areas.ProviderArea.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPhysicianRepository _physician;
        public HomeController(IPhysicianRepository physicianRepository)
        {
               _physician = physicianRepository;
        }
        [AuthorizationRepository("Physician")]
        [Area("ProviderArea")]
        public IActionResult PhysicianTabsLayout()
        {
            var aspnetid = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().ReadJwtToken(Request.Cookies["jwt"]).Claims.FirstOrDefault(x => x.Type == "AspNetId").Value;

            if (aspnetid == null)
            {
                return RedirectToAction("AdminLogin", "Home", new { area = "AdminArea" });
            }

            _physician.SetSession(_physician.GetFirstOrDefault(x => x.Aspnetuserid == aspnetid));
            return View();
        }
    }
}
