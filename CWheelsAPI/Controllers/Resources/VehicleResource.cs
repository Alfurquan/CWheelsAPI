using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Controllers.Resources
{
    public class VehicleResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public string Color { get; set; }
        public string Company { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsHotAndNew { get; set; }
        public bool IsFeatured { get; set; }
        public string Location { get; set; }
        public string Condition { get; set; }
        public ContactResource Contact { get; set; }
        public KeyValuePairResource Category { get; set; }
        public ICollection<PhotoResource> Images { get; set; }

        public VehicleResource()
        {
            Images = new Collection<PhotoResource>();
        }
    }
}
