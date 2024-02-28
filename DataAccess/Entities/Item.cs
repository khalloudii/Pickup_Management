using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public int InitialBalance { get; set; }
        public int CurrentBalance { get; set; }
        public double Amount { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
