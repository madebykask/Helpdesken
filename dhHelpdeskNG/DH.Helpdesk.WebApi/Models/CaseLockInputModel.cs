using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.WebApi.Models
{
    public class CaseLockInputModel
    {
        [Required]
        public int CaseId { get; set; }

        [Required]
        public string SessionId { get; set; }
    }
}