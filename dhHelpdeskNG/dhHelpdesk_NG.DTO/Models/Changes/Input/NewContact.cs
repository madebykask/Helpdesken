namespace DH.Helpdesk.BusinessData.Models.Changes.Input
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewContact : IBusinessModelWithId
    {
        public NewContact(int changeId, string name, string phone, string email, string company, DateTime createdDateAndTime)
        {
            this.ChangeId = changeId;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Company = company;
            this.CreatedDateAndTime = createdDateAndTime;
        }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }

        public string Company { get; private set; }

        public DateTime CreatedDateAndTime { get; private set; }

        [IsId]
        public int ChangeId { get; private set; }

        [IsId]
        public int Id { get; set; }
    }
}
