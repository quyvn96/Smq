using Microsoft.AspNet.Identity.EntityFramework;
using Smq.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smq.Model.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(): base()
        {

        }
        [StringLength(250)]
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(256)]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [MaxLength(256)]
        public string UpdatedBy { get; set; }
        public bool Status { get; set; }
    }
}
