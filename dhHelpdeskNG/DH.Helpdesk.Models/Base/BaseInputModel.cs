using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Models.Base
{
    public class BaseInputModel
    {
        [Required]
        public int Cid { get; set; } //CustomerId
    }
}
