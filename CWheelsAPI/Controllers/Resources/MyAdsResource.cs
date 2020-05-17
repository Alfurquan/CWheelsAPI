using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Controllers.Resources
{
    public class MyAdsResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Model { get; set; }
        public string Company { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsFeatured { get; set; }
        public string Location { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
