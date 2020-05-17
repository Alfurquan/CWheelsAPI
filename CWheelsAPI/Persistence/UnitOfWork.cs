using CWheelsAPI.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CWheelsDbContext _context;

        public UnitOfWork(CWheelsDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            Accounts = new AccountsRepository(_context);
            Vehicles = new VehicleRepository(_context);
            Thumbnails = new ThumbnailRepository(_context);
        }

        public ICategoryRepository Categories { get; private set; }

        public IAccountsRepository Accounts { get; private set; }

        public IVehicleRepository Vehicles { get; private set; }

        public IThumbnailRepository Thumbnails { get; private set; }

        public Task CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
