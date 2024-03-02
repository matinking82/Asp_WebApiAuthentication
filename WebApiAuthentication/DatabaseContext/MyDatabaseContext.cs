using Microsoft.EntityFrameworkCore;
using WebApiAuthentication.Models;

namespace WebApiAuthentication.DatabaseContext
{
    public class MyDatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options) : base(options)
        {

        }
    }
}
