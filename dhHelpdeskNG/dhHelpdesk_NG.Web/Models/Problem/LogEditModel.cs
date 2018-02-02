using System.Collections.Generic;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Models.Problem
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class LogEditModel
    {
        public int Id { get; set; }

        public int ChangedByUserId { get; set; }

        [Required]
        [StringLength(2000)]
        [DisplayName("Log Note")]
        public string LogText { get; set; }

        public bool InternNotering { get; set; }

        public bool ExternNotering { get; set; }

        public int? FinishingCauseId { get; set; }

        public DateTime? FinishingDate { get; set; }

        public bool FinishConnectedCases { get; set; }

        public int ProblemId { get; set; }

        public IList<FinishingCause> FinishingCauses { get; set; }

        public string FinishingCause { get; set; }
    }
}