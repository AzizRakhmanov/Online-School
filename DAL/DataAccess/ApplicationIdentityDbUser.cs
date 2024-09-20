using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataAccess
{
    public class ApplicationIdentityDbUser : IdentityDbContext<IdentityUser>
    {
        public ApplicationIdentityDbUser(DbContextOptions<ApplicationIdentityDbUser> options)
            : base(options)
        { }
    }
}
