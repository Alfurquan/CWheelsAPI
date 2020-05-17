using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

        public Category()
        {
            Vehicles = new Collection<Vehicle>();
        }
    }
}
