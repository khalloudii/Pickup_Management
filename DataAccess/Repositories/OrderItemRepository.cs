using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>
    {
        private readonly PickupContext _context;
        public OrderItemRepository(PickupContext context) : base(context)
        {
            _context = context;
        }
    }
}
