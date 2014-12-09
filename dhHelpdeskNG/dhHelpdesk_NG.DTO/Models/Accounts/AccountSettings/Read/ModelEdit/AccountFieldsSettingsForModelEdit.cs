namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit
{
    public class AccountFieldsSettingsForModelEdit
    {
        public AccountFieldsSettingsForModelEdit(
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

        public OrdererFieldSettings Orderer { get; private set; }

        public UserFieldSettings User { get; private set; }

        public AccountInformationFieldSettings AccountInformation { get; private set; }

        public ContactFieldSettings Contact { get; set; }

        public DeliveryInformationFieldSettings DeliveryInformation { get; private set; }

        public ProgramFieldSettings Program { get; private set; }

        public OtherFieldSettings Other { get; private set; }
    }
}
