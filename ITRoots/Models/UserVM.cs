using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITRoots.Models
{
    public class UserVM
    {
        public int ID { get; set; }

        [Required]
        [MinLength(3)]
        [Display(Name = "FullName", ResourceType = typeof(Resources.Resource))]
        public string FullName { get; set; }
        [Required]
        [MinLength(3)]
        [Display(Name = "Username", ResourceType = typeof(Resources.Resource))]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        public string Password { get; set; }


        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Resource))]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "IsEmailConfirmed", ResourceType = typeof(Resources.Resource))]

        public bool IsEmailConfirmed { get; set; }

       
        [Display(Name = "CreatedDate", ResourceType = typeof(Resources.Resource))]
        public System.DateTime CreatedDate { get; set; }

        public string ActivationCode { get; set; }

        [Display(Name = "UserType", ResourceType = typeof(Resources.Resource))]
        public string UserType { get; set; }
    }
}