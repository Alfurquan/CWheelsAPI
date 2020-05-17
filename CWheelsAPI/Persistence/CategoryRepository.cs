using CWheelsAPI.Core;
using CWheelsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Persistence
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CWheelsDbContext context) : base(context)
        {

        }
    }
}
