namespace dhHelpdesk_NG.Web.Models.Problem.Output
{
    using System.ComponentModel.DataAnnotations;

    public class LogEditModel
    {
        public int Id { get; set; }

        public int ChangedByUserId { get; set; }

        public string LogText { get; set; }

        public int ShowOnCase { get; set; }

        public int? FinishingCauseId { get; set; }

        public string FinishingDate { get; set; }

        public bool FinishConnectedCases { get; set; }
    }
}