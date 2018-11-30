using System;
using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.WebApi.Models
{
    public class CaseUnLockInputModel
    {
        [Required]
        public int CaseId { get; set; }

        [Required]
        public Guid LockGuid { get; set; }
    }
}