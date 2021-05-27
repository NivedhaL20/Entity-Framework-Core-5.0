using Microsoft.EntityFrameworkCore;

namespace EFCore5
{
    public class UserContext : DbContext
    {
        public UserContext() { }

        public UserContext(DbContextOptions<UserContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
