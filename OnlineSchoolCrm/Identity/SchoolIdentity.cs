using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OnlineSchoolCrm.Identity
{
    public class SchoolIdentity : IdentityDbContext<IdentityUser>
    {
    }
}
