using System.ComponentModel.DataAnnotations;
using DH.Helpdesk.Models.Base;

namespace DH.Helpdesk.Models.Case
{
    public class GetCaseFileInputModel : BaseInputModel
    {
        [Required]
        public int CaseId { get; set; }

        [Required]
        public int FileId { get; set; }
    }
}