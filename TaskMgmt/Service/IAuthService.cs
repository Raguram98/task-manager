using TaskMgmt.DTO;

namespace TaskMgmt.Service
{
    public interface IAuthService
    {
        Task<string?> RegisterAsync(RegisterDto req);
        Task<string?> LoginAsync(LoginDto req);
    }
}
