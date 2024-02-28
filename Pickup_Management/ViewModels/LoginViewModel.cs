using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pickup_Management.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "البريد الإلكتروني")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Display(Name = "ذكرني?")]
        public bool RememberMe { get; set; }
    }
}