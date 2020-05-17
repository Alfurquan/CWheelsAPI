using CWheelsAPI.Core;
using CWheelsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Persistence
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(CWheelsDbContext context) : base(context)
        {
        }

        public async Task<List<Vehicle>> GetHotAndNewVehicles()
        {
           return await CWheelsDbContext.Vehicles
                 .Where(v => v.IsHotAndNew == true)
                 .Include(vf => vf.Thumbnails)
                 .ToListAsync();
        }

        public Vehicle GetVehicleDetails(int id)
        {
           return  CWheelsDbContext.Vehicles
                .Where(v => v.Id == id)
                .Include(vf => vf.User)
                .Include(vf => vf.Images)
                .Include(vf => vf.Category)
                .SingleOrDefault();
        }


        public async Task<List<Vehicle>> GetVehiclesByCategory(int categoryId)
        {
            return await CWheelsDbContext.Vehicles
                .Where(v => v.CategoryId == categoryId)
                .Include(vf => vf.Thumbnails)
                .ToListAsync();
        }

        public async Task<List<Vehicle>> GetVehiclesForUser(User user)
        {
            return await CWheelsDbContext.Vehicles
                .Where(v => v.UserId == user.Id)
                .Include(vf => vf.Thumbnails)
                .OrderByDescending(v => v.DatePosted)
                .ToListAsync();
        }

        public async Task<List<Vehicle>> SearchVehicles(string searchPattern)
        {
            return await CWheelsDbContext.Vehicles.Where(v => v.Title.StartsWith(searchPattern)).ToListAsync();
        }

        public Vehicle GetFullVehicleDetails(int id)
        {
            return CWheelsDbContext.Vehicles
               .Where(v => v.Id == id)
               .Include(vf => vf.User)
               .Include(vf => vf.Images)
               .Include(vf => vf.Thumbnails)
               .Include(vf => vf.Category)
               .SingleOrDefault();
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
