namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class AccountInformationFieldSettings
    {
        public AccountInformationFieldSettings()
        {
        }

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
        public FieldSetting StartedDate { get;  set; }

        [LocalizedDisplay("Slutdatum")]
        public FieldSetting FinishDate { get;  set; }

        [LocalizedDisplay("E-posttyp")]
        public FieldSetting EMailTypeId { get;  set; }

        [LocalizedDisplay("Hemkatalog")]
        public FieldSetting HomeDirectory { get;  set; }

        [LocalizedDisplay("Profil")]
        public FieldSetting Profile { get;  set; }

        [LocalizedDisplay("Inventarienummer")]
        public FieldSetting InventoryNumber { get;  set; }

        [LocalizedDisplay("Vallista 1")]
        public FieldSetting AccountTypeId { get;  set; }

        [LocalizedDisplay("Vallista 2")]
        public FieldSetting AccountType2 { get;  set; }

        [LocalizedDisplay("Vallista 3")]
        public FieldSetting AccountType3 { get;  set; }

        [LocalizedDisplay("Vallista 4")]
        public FieldSetting AccountType4 { get;  set; }

        [LocalizedDisplay("Vallista 5")]
        public FieldSetting AccountType5 { get;  set; }

        [LocalizedDisplay("Övrigt")]
        public FieldSetting Info { get;  set; }
    }
}