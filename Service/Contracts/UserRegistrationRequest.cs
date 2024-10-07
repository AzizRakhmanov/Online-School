using System.ComponentModel.DataAnnotations;

namespace Service.Contracts
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
