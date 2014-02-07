namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using DH.Helpdesk.BusinessData.Models.Common.Input;

    public sealed class Contact : INewBusinessModel
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Company { get; set; }

        public int Id { get; set; }
    }
}
