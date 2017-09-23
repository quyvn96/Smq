﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smq.Web.Models
{
    public class FeedbackViewModel
    {
        public int ID { get; set; }
        [MaxLength(250,ErrorMessage="Tên không vượt quá 250 ký tự")]
        [Required(ErrorMessage="Phải nhập tên")]
        public string Name { get; set; }
        [MaxLength(250,ErrorMessage="Email không vượt quá 250 ký tự")]
        public string Email { get; set; }
        [MaxLength(500,ErrorMessage="Tin nhắn không vượt quá 500 ký tự")]
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Phải nhập trạng thái")]
        public bool Status { get; set; }

        public ContactDetailViewModel ContactDetail { get; set; }
    }
}