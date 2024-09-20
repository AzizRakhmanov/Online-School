using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace OnlineSchoolCrm.Identity
{
    public class SchoolIdentity : IdentityDbContext<IdentityUser>
    {
    }
}
