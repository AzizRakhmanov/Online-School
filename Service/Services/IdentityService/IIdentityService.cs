using Service.Contracts.Requests;
using Service.Contracts.Responses;

namespace Service.Services.IdentityService
{
    public interface IIdentityService
    {
        public Task<AuthenticationResult> LoginAsync(string email, string password);
        public Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request);
    }
}
