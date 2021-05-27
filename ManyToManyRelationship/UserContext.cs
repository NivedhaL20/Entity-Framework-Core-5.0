using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;

namespace ManyToManyRelationship
{
    public class UserContext : DbContext
    {      
        //Runtime configuration
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        //Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
    }

    // Many to Many mapping between User and Group
    public class User
    {
        public string Name { get; set; }
        public int Id { get; set; }  
        
        public ICollection<Group> Groups { get; set; }
    }

    public class Group
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
