namespace DH.Helpdesk.BusinessData.Models.Problem.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewProblemLogDto : INewBusinessModel
    {
        public NewProblemLogDto(int changedByUserId, string logText, int showOnCase, int? finishingCauseId, DateTime? finishingDate, int finishConnectedCases)
        {
            this.ChangedByUserId = changedByUserId;
            this.LogText = logText;
            this.ShowOnCase = showOnCase;
            this.FinishingCauseId = finishingCauseId;
            this.FinishingDate = finishingDate;
            this.FinishConnectedCases = finishConnectedCases;
        }

        public int Id { get; set; }

        [IsId]
        public int ProblemId { get; set; }

        [IsId]
        public int ChangedByUserId { get; set; }

        [NotNullAndEmpty]
        public string LogText { get; set; }

        public int ShowOnCase { get; set; }

        [IsId]
        public int? FinishingCauseId { get; set; }

        public DateTime? FinishingDate { get; set; }

        public int FinishConnectedCases { get; set; }
    }
}