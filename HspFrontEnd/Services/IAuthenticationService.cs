using HspFrontEnd.Models;

namespace HspFrontEnd.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationStatus> RegisterAsync(RegisterDto register);
        Task<AuthenticationStatus> LoginAsync(LoginDto login);
        Task LogoutAsync();
    }
}
