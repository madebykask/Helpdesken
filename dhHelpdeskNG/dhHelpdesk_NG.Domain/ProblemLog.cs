using System;

namespace dhHelpdesk_NG.Domain
{
    public class ProblemLog : Entity
    {
        public int ChangedByUser_Id { get; set; }
        public int FinishConnectedCases { get; set; }
        public int? FinishingCause_Id { get; set; }
        public int Problem_Id { get; set; }
        public int ShowOnCase { get; set; }
        public string LogText { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishingDate { get; set; }
        public Guid ProblemLogGUID { get; set; }

        public virtual FinishingCause FinishingCause { get; set; }
        public virtual Problem Problem { get; set; }
        public virtual User ChangedByUser { get; set; }
    }
}
