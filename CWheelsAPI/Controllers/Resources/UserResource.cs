using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Controllers.Resources
{
    public class UserResource
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public ICollection<VehicleResource> Vehicles { get; set; }

        public UserResource()
        {
            Vehicles = new Collection<VehicleResource>();
        }
    }
}
