using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Models
{
    public class Thumbnail
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string ThumbnailUrl { get; set; }
        public int VehicleId { get; set; }
    }
}
