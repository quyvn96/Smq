using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smq.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Please enter fullname")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please enter username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [MinLength(6,ErrorMessage= "Passwords must be at least 6 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage="Invalid email ")]
        public string Email { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter phone number")]
        public string PhoneNumber { get; set; }
    }
}