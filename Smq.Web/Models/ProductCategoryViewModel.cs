﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smq.Web.Models
{
    public class ProductCategoryViewModel
    {
        public int ID { set; get; }
        [Required(ErrorMessage="Please enter category name")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Please enter alias")]
        public string Alias { set; get; }
        public string Description { set; get; }

        public int? ParentID { set; get; }
        public int? DisplayOrder { set; get; }

        public string Image { set; get; }

        public bool? HomeFlag { set; get; }

        public virtual IEnumerable<PostViewModel> Posts { set; get; }

        public DateTime? CreatedDate { set; get; }


        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }


        public string UpdatedBy { set; get; }


        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }
        [Required(ErrorMessage = "Plase enter status")]
        public bool Status { set; get; }
    }
}