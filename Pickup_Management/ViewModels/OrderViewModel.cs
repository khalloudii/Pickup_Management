using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pickup_Management.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }

        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }

        [Required]
        public string FullName { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Email { get; set; }

        public List<SelectListItem> Items { get; set; }


        public string SelectedItemsWithNumbers { get; set; }

    }
}