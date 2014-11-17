namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write
{
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

        public FieldSetting StartedDate { get; private set; }

        public FieldSetting FinishDate { get; private set; }

        public FieldSetting EMailTypeId { get; private set; }

        public FieldSetting HomeDirectory { get; private set; }

        public FieldSetting Profile { get; private set; }

        public FieldSetting InventoryNumber { get; private set; }

        public FieldSetting AccountTypeId { get; private set; }

        public FieldSetting AccountType2 { get; private set; }

        public FieldSetting AccountType3 { get; private set; }

        public FieldSetting AccountType4 { get; private set; }

        public FieldSetting AccountType5 { get; private set; }

        public FieldSetting Info { get; private set; }
    }
}