namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class UpdatedContact
    {
        public UpdatedContact(
            int id,
            string name,
            string phone,
            string email,
            string company,
            DateTime changedDateAndTime)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Company = company;
            this.ChangedDateAndTime = changedDateAndTime;
        }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }

        public string Company { get; private set; }

        public DateTime ChangedDateAndTime { get; private set; }

        [IsId]
        public int Id { get; private set; }
    }
}
