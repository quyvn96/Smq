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
    }
}
