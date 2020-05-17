using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Controllers.Resources
{
    public class ThumbnailResource
    {
        public int Id { get; set; }
        public string ThumbnailUrl { get; set; }
        public int VehicleId { get; set; }
    }
}
