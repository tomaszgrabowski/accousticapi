namespace AccousticApi.DbContext
{
    using System.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Microsoft.Extensions.Configuration;

    public class AcUsersDbContext: DbContext
    {
        private readonly IConfiguration _configuration;

        public AcUsersDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<AcUser> AcUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AcUsersDatabase"));
        }
    }
}
