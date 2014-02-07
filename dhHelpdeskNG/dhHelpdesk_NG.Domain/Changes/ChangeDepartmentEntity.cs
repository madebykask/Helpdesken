namespace DH.Helpdesk.Domain.Changes
{
    public class ChangeDepartmentEntity
    {
        public int Change_Id { get; set; }

        public virtual ChangeEntity Change { get; set; }

        public int Department_Id { get; set; }

        public virtual Department Department { get; set; }
    }
}