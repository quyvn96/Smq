﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smq.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage="Nhập tên tài khoản")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Nhập tên mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}