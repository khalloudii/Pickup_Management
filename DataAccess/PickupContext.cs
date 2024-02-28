using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PickupContext : IdentityDbContext<AppUser>
    {
        public PickupContext() : base("DefaultConnection")
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static PickupContext Create()
        {
            return new PickupContext();
        }
    }
}
