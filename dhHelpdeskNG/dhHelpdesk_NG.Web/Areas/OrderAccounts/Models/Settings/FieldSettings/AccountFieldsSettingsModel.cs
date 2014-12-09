namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class AccountFieldsSettingsModel
    {
        public AccountFieldsSettingsModel(
            OrdererFieldSettings orderer,
            UserFieldSettings user,
            AccountInformationFieldSettings accountInformation,
            ContactFieldSettings contact,
            DeliveryInformationFieldSettings deliveryInformation,
            ProgramFieldSettings program,
            OtherFieldSettings other)
        {
            this.Orderer = orderer;
            this.User = user;
            this.AccountInformation = accountInformation;
            this.Contact = contact;
            this.DeliveryInformation = deliveryInformation;
            this.Program = program;
            this.Other = other;
        }

        [NotNull]
        public OrdererFieldSettings Orderer { get; private set; }

        [NotNull]
        public UserFieldSettings User { get; private set; }

        [NotNull]
        public AccountInformationFieldSettings AccountInformation { get; private set; }

        [NotNull]
        public ContactFieldSettings Contact { get; set; }

        [NotNull]
        public DeliveryInformationFieldSettings DeliveryInformation { get; private set; }

        [NotNull]
        public ProgramFieldSettings Program { get; private set; }

        [NotNull]
        public OtherFieldSettings Other { get; private set; }
    }
}
