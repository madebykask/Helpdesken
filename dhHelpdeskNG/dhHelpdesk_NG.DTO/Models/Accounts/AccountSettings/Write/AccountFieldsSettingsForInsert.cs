namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write
{
    using System;

    public sealed class AccountFieldsSettingsForInsert : AccountFieldsSettingsForWrite
    {
        public AccountFieldsSettingsForInsert(
            int activityId,
            OrdererFieldSettings orderer,
            UserFieldSettings user,
            AccountInformationFieldSettings accountInformation,
            DeliveryInformationFieldSettings deliveryInformation,
            ContactFieldSettings contact,
            ProgramFieldSettings program,
            OtherFieldSettings other,
            DateTime createdDate)
            : base(activityId, orderer, user, accountInformation, deliveryInformation, contact, program, other)
        {
            this.CreatedDate = createdDate;
        }

        public DateTime CreatedDate { get; private set; }
    }
}