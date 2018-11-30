using System;
using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.WebApi.Models
{
    public class ExtendCaseLockInputModel
    {
        [Required]
        public int CaseId { get; set; }

        [Required]
        public Guid LockGuid { get; set; }

        [Required]
        public int ExtendValue { get; set; }
    }
}