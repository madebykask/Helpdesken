namespace DH.Helpdesk.Domain
{
    public class LinkUser
    {
        public int Link_Id { get; set; }
        public int User_Id { get; set; }

        public virtual User User { get; set; }
    }
}
