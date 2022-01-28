using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITRoots.Models.AccountVM
{
    public class ConfirmEmailVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Code", ResourceType = typeof(Resources.Resource))]
        public string Code { get; set; }
    }
}