using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace DataAccess.ServiceRepository
{
    public class CustomConnectionIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor httpContext;
        public CustomConnectionIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor;
        }

        public string? GetUserId(HubConnectionContext connection)
        {
            try
            {
                var jwtservice = httpContext.HttpContext.RequestServices.GetService<IJwtRepository>();
                var request = httpContext.HttpContext.Request;
                var token = request.Cookies["jwt"];
                if (token == null)
                {
                    return null;
                }
                jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
                var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
                var id = roleClaim.Value;
                return id;
            }
            catch
            {
                return null;
            }

        }
    }
}

