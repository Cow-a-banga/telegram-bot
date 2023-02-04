using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class DatabaseContext: DbContext
    {
        public DbSet<UserDto> Users { get; set; }
        public DbSet<PaymentDto> Payments { get; set; }
        public DbSet<WhoAmIQuestionDto> Questions { get; set; }
        
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