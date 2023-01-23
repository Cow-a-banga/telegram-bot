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
            var builder = new NpgsqlConnectionStringBuilder();
            builder.Database = "grsitdyi";
            builder.Username = "grsitdyi";
            builder.Password = "DfxRF9J3VR5uKqmo20-4NDd9AOZ9aZqw";
            builder.Host = "rogue.db.elephantsql.com";
            
            optionsBuilder.UseNpgsql(
                //"Host=rogue.db.elephantsql.com;Port=5433;Database=grsitdyi;Username=grsitdyi;Password=DfxRF9J3VR5uKqmo20-4NDd9AOZ9aZqw"
                builder.ConnectionString
                );
        }
    }
}