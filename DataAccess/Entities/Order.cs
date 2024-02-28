using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Order : BaseEntity
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }


        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public double TotalAmount { get; set; }

        public string PaymentLink { get; set; } 
        public bool IsPaid { get; set; }

        public string UserName { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } 
    }
}
