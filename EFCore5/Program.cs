using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFCore5
{
    public class Program
    {
        private static readonly string ConnectionString = ""; //Add your connection string
        public static DbContextOptionsBuilder<UserContext> options =
            new DbContextOptionsBuilder<UserContext>().UseSqlServer(ConnectionString)
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        // Use the LogTo(Action<String>, LogLevel) overload for default logging of all events.

        public static void Main(string[] args)
        {
            Console.WriteLine("Preparing...");
            
            AddUser();

            //Get the count of Users
            ShowCounts(); 
            
            //Query String using Where Clause
            MapData();

            Console.ReadLine();
        }
        private static void AddUser()
        {            
            using var context = new UserContext(options.Options);

            var user = new User
            {
                Id = 1,
                Name = "Name"
            };
            context.Users.Add(user);
            context.SaveChanges();
        }
        private static void ShowCounts()
        {          
            using var context = new UserContext(options.Options);
            Console.WriteLine($"{nameof(context.Users)} - {context.Users.Count()}");            
        }
        private static void MapData()
        {           
            using var context = new UserContext(options.Options);

            var baseQuery = context.Users;
            var query = baseQuery.Where(user => user.Name == "Name")
                .Select(data => new
                {
                    data.Name,
                    data.Id
                });

            //To print Debug View Query
            var queryString = query.ToQueryString();            
            Console.WriteLine(queryString);

            foreach (var result in query)
            {                
                Console.WriteLine($"Name : {result.Name} , Id : {result.Id}");              
            }
        }
    }
}
