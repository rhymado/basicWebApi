using basicWebApi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace basicWebApi.Models
{
    public class BasicDbContext : DbContext
    {
        public BasicDbContext(DbContextOptions<BasicDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}