using CWheelsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Core
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<List<Vehicle>> GetHotAndNewVehicles();

        Task<List<Vehicle>> SearchVehicles(string searchPattern);

        Task<List<Vehicle>> GetVehiclesByCategory(int categoryId);

        Vehicle GetVehicleDetails(int id);

        Vehicle GetFullVehicleDetails(int id);

        Task<List<Vehicle>> GetVehiclesForUser(User user);
    }
}
