using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Quantity { get; set; }

        public int ItemID { get; set; }
        public virtual Item Item { get; set; }
        
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
