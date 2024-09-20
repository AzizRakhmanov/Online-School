using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataAccess
{
    public class SchoolDb : IdentityDbContext<IdentityUser>
    {
        public SchoolDb(DbContextOptions<SchoolDb> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
