using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smq.Web.Models
{
    public class ContactDetailViewModel
    {
        public int ID { get; set; }
        [Required( ErrorMessage="Please enter name")]
        public string Name { get; set; }
        [MaxLength(50,ErrorMessage= "Phone does not exceed 50 characters")]
        public string Phone { get; set; }
        [MaxLength(250, ErrorMessage = "Email does not exceed 50 characters")]
        public string Email { get; set; }
        [MaxLength(250, ErrorMessage = "Website does not exceed 50 characters")]
        public string Website { get; set; }
        [MaxLength(250, ErrorMessage = "Address does not exceed 50 characters")]
        public string Address { get; set; }
        public string Other { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public bool Status { get; set; }
    }
}