using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITRoots.Models
{
    public class LoginVM
    {
        [Required]
        [MinLength(3)]
        [Display(Name = "Username", ResourceType = typeof(Resources.Resource))]

        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        public string Password { get; set; }

        [Display(Name = "UserType", ResourceType = typeof(Resources.Resource))]

        public string UserType { get; set; }



    }
}