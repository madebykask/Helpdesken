namespace DH.Helpdesk.VBCSharpBridge.Models
{
    public class CaseBridge
    {
        public int Customer_Id { get; set; }

        public string FromEmail { get; set; }

        public int? Performer_User_Id { get; set; }

        public int? WorkingGroup_Id { get; set; }
    }
}
