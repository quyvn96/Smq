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
        [Required( ErrorMessage="Không được bỏ trống tên")]
        public string Name { get; set; }
        [MaxLength(50,ErrorMessage="Số điện thoại không vượt quá 50 ký tự")]
        public string Phone { get; set; }
        [MaxLength(250, ErrorMessage = "Email không vượt quá 50 ký tự")]
        public string Email { get; set; }
        [MaxLength(250, ErrorMessage = "Website không vượt quá 50 ký tự")]
        public string Website { get; set; }
        [MaxLength(250, ErrorMessage = "Địa chỉ không vượt quá 50 ký tự")]
        public string Address { get; set; }
        public string Other { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public bool Status { get; set; }
    }
}