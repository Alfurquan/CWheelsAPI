using CWheelsAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Controllers.Resources
{
    public class CategoryResource
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        public ICollection<VehicleResource> Vehicles { get; set; }

        public CategoryResource()
        {
            Vehicles = new Collection<VehicleResource>();
        }
    }
}
