using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataBase
{
    public class DatabaseContext: DbContext
    {
        public DbSet<UserDto> Users { get; set; }
        
        public DatabaseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = System.Environment.GetEnvironmentVariable("TGBOT_CONN");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}