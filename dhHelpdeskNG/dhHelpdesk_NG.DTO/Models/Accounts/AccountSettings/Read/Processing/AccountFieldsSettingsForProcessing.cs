namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing
{
    using System;

    public abstract class AccountFieldsSettingsForProcessing
    {
        protected AccountFieldsSettingsForProcessing(
            int activityId,
            OrdererFieldSettings orderer,
            UserFieldSettings user,
            AccountInformationFieldSettings accountInformation,
            DeliveryInformationFieldSettings deliveryInformation,
            ProgramFieldSettings program,
            OtherFieldSettings other,
            DateTime createdDate,
            DateTime changedDate)
        {
            this.ActivityId = activityId;
            this.Orderer = orderer;
            this.User = user;
            this.AccountInformation = accountInformation;
            this.DeliveryInformation = deliveryInformation;
            this.Program = program;
            this.Other = other;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
        }

        public int ActivityId { get; private set; }

        public OrdererFieldSettings Orderer { get; private set; }

        public UserFieldSettings User { get; private set; }

        public AccountInformationFieldSettings AccountInformation { get; private set; }

        public DeliveryInformationFieldSettings DeliveryInformation { get; private set; }

        public ProgramFieldSettings Program { get; private set; }

        public OtherFieldSettings Other { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }
    }
}
