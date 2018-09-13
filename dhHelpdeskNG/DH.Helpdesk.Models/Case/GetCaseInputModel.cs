using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Models.Base;

namespace DH.Helpdesk.Models.Case
{
    public class GetCaseInputModel: BaseInputModel
    {
        [Required]
        public int CaseId { get; set; }
    }
}
