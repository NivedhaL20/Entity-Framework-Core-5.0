using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ManyToManyRelationship
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
            var john = new User { Name = "John" };

            var football = new Group { Name = "Football", Users= new List<User> { jane, john } };
            var cricket = new Group { Name = "Cricket", Users = new List<User> { john } };

            context.AddRange(jane, john, football, cricket);
            await context.SaveChangesAsync();

            var users = await context.Users.Where(u => u.Groups.Any(m => m.Name == "Football")).ToListAsync();

            foreach (var user in users)
            {
                Console.WriteLine("User: " + user.Name);                
            }
            Console.ReadKey();
        }
    }
}
