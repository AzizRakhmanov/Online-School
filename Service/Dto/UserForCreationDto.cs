using Microsoft.AspNet.Identity.EntityFramework;

namespace Service.Dto
{
    public class UserForCreationDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserId { get; set; }

        public IdentityUser IdentityUser { get; set; }
    }
}
