using Feature.Users;
using Feature.Quizzes;
using Feature.Settings;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(Feature.Users.RefPoint).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(Feature.Quizzes.RefPoint).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(Feature.Settings.RefPoint).Assembly);
        }
    }
}




