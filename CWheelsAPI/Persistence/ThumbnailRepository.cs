using CWheelsAPI.Core;
using CWheelsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Persistence
{
    public class ThumbnailRepository : Repository<Thumbnail>, IThumbnailRepository
    {
        public ThumbnailRepository(CWheelsDbContext context) : base(context)
        {
        }

        public async Task<List<Thumbnail>> GetThumbnails(int vehicleId)
        {
            return await CWheelsDbContext.Thumbnails.Where(p => p.VehicleId == vehicleId).ToListAsync();
        }

        public CWheelsDbContext CWheelsDbContext
        {
            get
            {
                return context as CWheelsDbContext;
            }
        }
    }
}
