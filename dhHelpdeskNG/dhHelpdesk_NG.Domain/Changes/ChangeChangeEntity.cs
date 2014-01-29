namespace dhHelpdesk_NG.Domain.Changes
{
    public class ChangeChangeEntity
    {
        public int Change_Id { get; set; }

        public virtual ChangeEntity Change { get; set; }

        public int RelatedChange_Id { get; set; }

        public virtual ChangeEntity RelatedChange { get; set; }
    }
}
