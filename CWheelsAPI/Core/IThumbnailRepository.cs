using CWheelsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Core
{
    public interface IThumbnailRepository : IRepository<Thumbnail>
    {
        Task<List<Thumbnail>> GetThumbnails(int vehicleId);

    }
}
