namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class Contact : BusinessModel
    {
        public static Contact CreateNew(
            string name,
            string phone,
            string email,
            string company,
            DateTime createdDateAndTime)
        {
            return new Contact
                   {
                       Name = name,
                       Phone = phone,
                       Email = email,
                       Company = company,
                       CreatedDateAndTime = createdDateAndTime,
                       State = ModelStates.Created
                   };
        }

        public static Contact CreateUpdated(
            int id,
            int changeId,
            string name,
            string phone,
            string email,
            string company,
            DateTime changedDateAndTime)
        {
            return new Contact
                   {
                       Id = id,
                       Name = name,
                       Phone = phone,
                       Email = email,
                       Company = company,
                       ChangedDateAndTime = changedDateAndTime,
                       State = ModelStates.Updated
                   };
        }

        public static Contact CreateExisting(int id, string name, string phone, string email, string company)
        {
            return new Contact
                   {
                       Id = id,
                       Name = name,
                       Phone = phone,
                       Email = email,
                       Company = company,
                       State = ModelStates.ForEdit
                   };
        }

        private Contact()
        {
        }

        [IsId]
        public int ChangeId { get; internal set; }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }

        public string Company { get; private set; }

        [AllowRead(ModelStates.Created)]
        public DateTime CreatedDateAndTime { get; private set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDateAndTime { get; private set; }
    }
}