using System;
using System.ComponentModel.DataAnnotations;

namespace EFCore5
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }       
    }
}
