using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ItemRepository : GenericRepository<Item>
    {
        private readonly PickupContext _context;
        public ItemRepository(PickupContext context) : base(context)
        {
            _context = context;
        }
    }
}
