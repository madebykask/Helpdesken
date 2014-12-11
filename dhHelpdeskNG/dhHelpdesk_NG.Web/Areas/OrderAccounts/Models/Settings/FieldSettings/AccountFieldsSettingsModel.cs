namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class AccountFieldsSettingsModel
    {
        public AccountFieldsSettingsModel()
        {
        }

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
        public OrdererFieldSettings Orderer { get; set; }

        [NotNull]
        public UserFieldSettings User { get;  set; }

        [NotNull]
        public AccountInformationFieldSettings AccountInformation { get;  set; }

        [NotNull]
        public ContactFieldSettings Contact { get; set; }

        [NotNull]
        public DeliveryInformationFieldSettings DeliveryInformation { get;  set; }

        [NotNull]
        public ProgramFieldSettings Program { get;  set; }

        [NotNull]
        public OtherFieldSettings Other { get;  set; }
    }
}
