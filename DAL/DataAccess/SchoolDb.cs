using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataAccess
{
    public class SchoolDb : DbContext
    {
        public SchoolDb(DbContextOptions<SchoolDb> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
