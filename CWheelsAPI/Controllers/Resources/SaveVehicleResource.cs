using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Controllers.Resources
{
    public class SaveVehicleResource
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [MaxLength(255)]
        public string Model { get; set; }
        [Required]
        [MaxLength(255)]
        public string Engine { get; set; }
        [Required]
        [MaxLength(255)]
        public string Color { get; set; }
        [Required]
        [MaxLength(255)]
        public string Company { get; set; }
        [Required]
        public bool IsFeatured { get; set; }
        [Required]
        [MaxLength(255)]
        public string Location { get; set; }
        [Required]
        [MaxLength(255)]
        public string Condition { get; set; }
        [Required]
        public int CategoryId { get; set; }
      
    }
}
