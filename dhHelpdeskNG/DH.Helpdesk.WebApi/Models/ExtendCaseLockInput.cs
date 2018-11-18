using System;
using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.WebApi.Models
{
    public class ExtendCaseLockInputModel
    {
        [Required]
        public Guid LockGuid { get; set; }

        [Required]
        public int ExtendValue { get; set; }
    }
}