namespace DH.Helpdesk.Domain.Changes
{
    public class ChangeChangeGroupEntity
    {
        public int Change_Id { get; set; }

        public virtual ChangeEntity Change { get; set; }

        public int ChangeGroup_Id { get; set; }

        public ChangeGroupEntity ChangeGroup { get; set; }
    }
}
