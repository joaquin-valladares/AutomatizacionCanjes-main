using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CANJES.Models.ViewModels
{
    public class ClsRecoveryPassViewModel
    {
        public string token { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        [Compare("password")]
        public string Confirm_password { get; set; }
    }
}