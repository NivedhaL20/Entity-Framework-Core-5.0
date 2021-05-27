using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TablePerHierarchy
{
    public class Program
    {
        private static readonly string ConnectionString = ""; //Add your connection string
        public static DbContextOptionsBuilder<UserContext> options = new DbContextOptionsBuilder<UserContext>()
                .UseSqlServer(ConnectionString)
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging();

        public static async Task Main(string[] args)
        {          
            using var context = new UserContext(options.Options);
            
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var jane = new User { Name = "Jane" };
            var john = new ExternalUser { Name = "John", ExternalCompany="Company" };

            var football = new Group { Name = "Football", Users = new List<User> { jane, john } };
            var cricket = new Group { Name = "Cricket", Users = new List<User> { jane } };

            context.AddRange(jane, john, football, cricket);
            await context.SaveChangesAsync();

            var users = await context.Set<ExternalUser>().Where(u => u.ExternalCompany == "Company").ToListAsync();
            foreach(var user in users)
            {
                Console.WriteLine("User:" + user.Name);
            }
        }
    }

    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ToTable method creates different table for ExternalUser entity - Table per Type
            
            //Without ToTable method, then the properties of TimeRestrictedUsers comes under User table as its inheritance hierarchy based structure - Table per Hierarchy

            modelBuilder.Entity<ExternalUser>().ToTable("ExternalUsers"); 
            modelBuilder.Entity<TimeRestrictedUser>(); 
        }
    }

    public class User
    {
        public string Name { get; set; }
        public int Id { get; set; }       
        public ICollection<Group> Groups { get; set; }
    }

    
    public class ExternalUser: User
    {
        public string ExternalCompany { get; set; }
    }

    public class TimeRestrictedUser: User
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
    public class Group
    {
        public string Name { get; set; }
        public int Id { get; set; }       
        public ICollection<User> Users { get; set; }
    }
}
