namespace dhHelpdesk_NG.Domain.Problems
{
    using global::System;

    public class ProblemEMailLog : Entity
    {
        public int MailID { get; set; }
        public int ProblemLog_Id { get; set; }
        public string EMailAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ProblemEMailLogGUID { get; set; }

        public virtual ProblemLog ProblemLog { get; set; }
    }
}
