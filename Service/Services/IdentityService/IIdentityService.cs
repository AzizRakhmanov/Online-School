using Service.Contracts;

namespace Service.Services.IdentityService
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);

        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}
