using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smq.Web.Models
{
    public class FeedbackViewModel
    {
        public int ID { get; set; }
        [MaxLength(250,ErrorMessage= "Name does not exceed 250 characters")]
        [Required(ErrorMessage="Please enter name")]
        public string Name { get; set; }
        [MaxLength(250,ErrorMessage= "Email does not exceed 250 characters")]
        public string Email { get; set; }
        [MaxLength(500,ErrorMessage= "Message does not exceed 500 characters")]
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Please enter status")]
        public bool Status { get; set; }

        public ContactDetailViewModel ContactDetail { get; set; }
    }
}