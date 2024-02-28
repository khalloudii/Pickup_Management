using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pickup_Management.ViewModels
{
    public class InvoiceViewModel
    {
        public string id { get; set; }
        public string status { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string amount_format { get; set; }
        public string url { get; set; }
        public string callback_url { get; set; }
        public DateTime? expired_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}