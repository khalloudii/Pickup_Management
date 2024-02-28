using DataAccess.Data;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        private readonly PickupContext _context;
        public OrderRepository(PickupContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrdersByUserNameAsync(string userName)
        {
            return await _context.Orders.Where(c => c.UserName == userName).ToListAsync();
        }
    }
}
