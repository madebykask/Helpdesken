namespace DH.Helpdesk.EForm.Model.Entities
{
    public class StateSecondary
    {
        public int Id { get; set; }
        public int StateSecondaryId { get; set; }
        public int WorkingGroupId { get; set; }
        public int MailId { get; set; }
        public int IncludeInCaseStatistics { get; set; }
        public int RecalculateWatchDate { get; set; }
    }
}
