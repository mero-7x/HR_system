using HRSYS.Domain.Entities;

namespace HRSYS.Domain.Repositories
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
