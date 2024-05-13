using System;
using System.IdentityModel.Tokens.Jwt;
using DataAccess.ServiceRepository.IServiceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

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
            var jwtservice = httpContext.HttpContext.RequestServices.GetService<IJwtRepository>();
            var request = httpContext.HttpContext.Request;
            var token = request.Cookies["jwt"];
            jwtservice.ValidateToken(token, out JwtSecurityToken jwttoken);
            var roleClaim = jwttoken.Claims.FirstOrDefault(x => x.Type == "AspNetId");
            var id = roleClaim.Value;
            return id;
        }
    }
}

