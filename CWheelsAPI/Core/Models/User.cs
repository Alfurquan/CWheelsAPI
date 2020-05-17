using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

        public User()
        {
            Vehicles = new Collection<Vehicle>();
        }
    }
}
