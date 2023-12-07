using AmberEnrollmentAPI.Models;

namespace AmberEnrollmentAPI.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        Task<bool> Login(User user);
        Task<bool> RegisterUser(User user);
    }
}