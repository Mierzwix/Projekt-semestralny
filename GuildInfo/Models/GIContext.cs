using System;
using System.Data.Entity;
using System.Linq;

namespace GuildInfo.Models
{
    /// <summary>
    /// Kontekst do bazy danych
    /// </summary>
    public class GIContext : DbContext
    {
        public GIContext()
            : base("data source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=GuildInfo.db;integrated security=True;connect timeout=30;MultipleActiveResultSets=True;App=EntityFramework")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GIContext, GuildInfo.Migrations.Configuration>());
        }

        public DbSet<Profession> Professions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Race> Races { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}