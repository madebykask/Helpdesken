namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    public sealed class Contact : IBusinessModelWithId
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Company { get; set; }

        public int Id { get; set; }
    }
}
