namespace DH.Helpdesk.Domain
{
    public class ChecklistRow : Entity
    {
        public int Case_Id { get; set; }
        public int ChecklistAction_Id { get; set; }
        public int Checklist_Id { get; set; }
        public string RowLog { get; set; }
    }
}
