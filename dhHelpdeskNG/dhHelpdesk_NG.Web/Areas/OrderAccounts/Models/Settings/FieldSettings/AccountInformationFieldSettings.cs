namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class AccountInformationFieldSettings
    {
        public AccountInformationFieldSettings(
            FieldSetting startedDate,
            FieldSetting finishDate,
            FieldSetting eMailTypeId,
            FieldSetting homeDirectory,
            FieldSetting profile,
            FieldSetting inventoryNumber,
            FieldSetting accountTypeId,
            FieldSetting accountType2,
            FieldSetting accountType3,
            FieldSetting accountType4,
            FieldSetting accountType5,
            FieldSetting info)
        {
            this.StartedDate = startedDate;
            this.FinishDate = finishDate;
            this.EMailTypeId = eMailTypeId;
            this.HomeDirectory = homeDirectory;
            this.Profile = profile;
            this.InventoryNumber = inventoryNumber;
            this.AccountTypeId = accountTypeId;
            this.AccountType2 = accountType2;
            this.AccountType3 = accountType3;
            this.AccountType4 = accountType4;
            this.AccountType5 = accountType5;
            this.Info = info;
        }

        [LocalizedDisplay("Startdatum")]
        public FieldSetting StartedDate { get; private set; }

        [LocalizedDisplay("Slutdatum")]
        public FieldSetting FinishDate { get; private set; }

        [LocalizedDisplay("E-posttyp")]
        public FieldSetting EMailTypeId { get; private set; }

        [LocalizedDisplay("Hemkatalog")]
        public FieldSetting HomeDirectory { get; private set; }

        [LocalizedDisplay("Profil")]
        public FieldSetting Profile { get; private set; }

        [LocalizedDisplay("Inventarienummer")]
        public FieldSetting InventoryNumber { get; private set; }

        [LocalizedDisplay("Vallista 1")]
        public FieldSetting AccountTypeId { get; private set; }

        [LocalizedDisplay("Vallista 2")]
        public FieldSetting AccountType2 { get; private set; }

        [LocalizedDisplay("Vallista 3")]
        public FieldSetting AccountType3 { get; private set; }

        [LocalizedDisplay("Vallista 4")]
        public FieldSetting AccountType4 { get; private set; }

        [LocalizedDisplay("Vallista 5")]
        public FieldSetting AccountType5 { get; private set; }

        [LocalizedDisplay("Övrigt")]
        public FieldSetting Info { get; private set; }
    }
}