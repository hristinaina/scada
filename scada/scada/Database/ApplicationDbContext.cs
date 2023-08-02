using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using scada.Models;
using Pomelo.EntityFrameworkCore.MySql;

namespace scada.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AlarmHistory> AlarmHistories { get; set; }

        public DbSet<TagHistory> TagHistories { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=localhost;port=3306;user=root;password=siit2020;database=SCADA;";
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0)));
        }
    }
}
