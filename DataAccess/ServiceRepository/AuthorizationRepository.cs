using DataAccess.Repository.IRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace DataAccess.ServiceRepository
{
    public class AuthorizationRepository : Attribute, IAuthorizatoinRepository, IAuthorizationFilter
    {
        private readonly string _role;

        public AuthorizationRepository(string role = "")
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var jwtservice = context.HttpContext.RequestServices.GetService<IJwtRepository>();
            var adminservice = context.HttpContext.RequestServices.GetService<IAdminRepository>();
            var physicianservice = context.HttpContext.RequestServices.GetService<IPhysicianRepository>();
            if (jwtservice == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AdminLogin", area = "AdminArea" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token == null || !jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "ExpirePopUp", area = "AdminArea" }));
                return;
            }

            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "Role");
            var aspnetid = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId").Value;

            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AdminLogin", area = "AdminArea" }));
                return;
            }

            if (string.IsNullOrEmpty(_role) || !_role.Contains(roleClaim.Value))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AdminLogin", area = "AdminArea" }));
                return;
            }

            if (roleClaim.Value == "Physician")
            {
                var physician = physicianservice.GetFirstOrDefault(x => x.Aspnetuserid == aspnetid);
                physicianservice.SetSession(physician);
            }
            if (roleClaim.Value == "Admin")
            {
                var admin = adminservice.GetFirstOrDefault(x => x.Aspnetuserid == aspnetid);
                adminservice.SetSession(admin);
            }









            //var person = SessionUtilsRepository.GetLoggedInPerson(context.HttpContext.Session);
            //if (person == null)
            //{
            //    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AdminLogin" }));
            //    return;
            //}
            //if (!string.IsNullOrEmpty(_role))
            //{
            //    if (!(person.Role == _role))
            //    {
            //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AdminLogin" }));
            //    }
            //}

        }
    }
}
