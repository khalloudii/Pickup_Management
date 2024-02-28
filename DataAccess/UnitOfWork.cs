using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork 
    {
        private readonly PickupContext _context;
        public UnitOfWork()
        {
            _context = new PickupContext();
        }

        //// Add All repositories handles here..
        private OrderRepository _orderRepository;
        private ItemRepository _itemRepository;
        private OrderItemRepository _orderItemRepository;

        //// Add All repositories getters here..
        public OrderRepository OrderRepository
        {
            get
            {
                if (this._orderRepository == null)
                {
                    this._orderRepository = new OrderRepository(_context);
                }
                return _orderRepository;
            }
        }
        public ItemRepository ItemRepository
        {
            get
            {
                if (this._itemRepository == null)
                {
                    this._itemRepository = new ItemRepository(_context);
                }
                return _itemRepository;
            }
        }
        public OrderItemRepository OrderItemRepository
        {
            get
            {
                if (this._orderItemRepository == null)
                {
                    this._orderItemRepository = new OrderItemRepository(_context);
                }
                return _orderItemRepository;
            }
        }


        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
