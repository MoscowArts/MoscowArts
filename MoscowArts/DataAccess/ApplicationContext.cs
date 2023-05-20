using Microsoft.EntityFrameworkCore;
using MoscowArts.Entities;

namespace MoscowArts.DataAccess
{
    public class ApplicationContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ApplicationContext(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> Users { get; set; }
    }
}
