namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write
{
    using System;

    public sealed class AccountFieldsSettingsForUpdate : AccountFieldsSettingsForWrite
    {
        public AccountFieldsSettingsForUpdate(
            int activityId,
            OrdererFieldSettings orderer,
            UserFieldSettings user,
            AccountInformationFieldSettings accountInformation,
            DeliveryInformationFieldSettings deliveryInformation,
            ProgramFieldSettings program,
            OtherFieldSettings other,
            DateTime changedDate)
            : base(activityId, orderer, user, accountInformation, deliveryInformation, program, other)
        {
            this.ChangedDate = changedDate;
        }

        public DateTime ChangedDate { get; private set; }
    }
}