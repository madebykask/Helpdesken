namespace dhHelpdesk_NG.DTO.DTOs.Problem.Output
{
    using System;

    public class ProblemLogOverview
    {
        public int Id { get; set; }

        public int ChangedByUserId { get; set; }

        public DateTime ChangedDate { get; set; }
    }
}