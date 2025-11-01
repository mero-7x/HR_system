using System.Security.Claims;
using HRSYS.Domain.Enums;
using HRSYS.Domain.Repositories; 
using Microsoft.AspNetCore.Http;

namespace HRSYS.Infrastructure.Services
{
    public class CurrentUserService: ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null) return null;
                return int.TryParse(claim.Value, out var id) ? id : null;
            }
        }

        public int? EmployeeId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("EmployeeId");
                if (claim == null) return null;
                return int.TryParse(claim.Value, out var id) ? id : null;
            }
        }

        public string? Username =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

        public Role? Role
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
                if (Enum.TryParse<Role>(claim, out var parsed))
                    return parsed;
                return null;
            }
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
