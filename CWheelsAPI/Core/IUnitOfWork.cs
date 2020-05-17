using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Core
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }

        IAccountsRepository Accounts { get; }

        IVehicleRepository Vehicles { get; }

        IThumbnailRepository Thumbnails { get; }

        Task CompleteAsync();
    }
}
