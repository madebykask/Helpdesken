using System.ComponentModel.DataAnnotations;
using DH.Helpdesk.Models.Base;

namespace DH.Helpdesk.Models.Case
{
    public class GetCaseInputModel: BaseInputModel
    {
        [Required]
        public int CaseId { get; set; }
        [Required]
        public int LangId { get; set; }
    }
}
