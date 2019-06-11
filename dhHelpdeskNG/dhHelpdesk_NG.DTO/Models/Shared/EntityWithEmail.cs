namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public class EntityWithEmail : EntityOverview
    {
        public EntityWithEmail()
        {
        }

        public EntityWithEmail(int id, string name, string email) : base(id, name)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}