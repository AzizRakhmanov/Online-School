using Domain.Commons;

namespace Domain.Models
{
    public class User : Auditable
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
