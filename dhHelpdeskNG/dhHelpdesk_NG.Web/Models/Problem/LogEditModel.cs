namespace dhHelpdesk_NG.Web.Models.Problem
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class LogEditModel
    {
        public int Id { get; set; }

        public int ChangedByUserId { get; set; }

        [Required]
        [StringLength(2000)]
        public string LogText { get; set; }

        public int ShowOnCase { get; set; }

        public int? FinishingCauseId { get; set; }

        public DateTime? FinishingDate { get; set; }

        public bool FinishConnectedCases { get; set; }
    }
}