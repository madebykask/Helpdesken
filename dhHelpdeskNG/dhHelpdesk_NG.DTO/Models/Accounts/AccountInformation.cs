namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using System;

    public sealed class AccountInformation
    {
        public AccountInformation(
            DateTime? startedDate,
            DateTime? finishDate,
            int eMailTypeId,
            int homeDirectory,
            int profile,
            string inventoryNumber,
            int accountTypeId,
            int accountType2,
            string accountType3,
            int accountType4,
            int accountType5,
            string info)
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

        public DateTime? StartedDate { get; private set; }

        public DateTime? FinishDate { get; private set; }

        public int EMailTypeId { get; private set; }

        public int HomeDirectory { get; private set; }

        public int Profile { get; private set; }

        public string InventoryNumber { get; private set; }

        public int AccountTypeId { get; private set; }

        public int AccountType2 { get; private set; }

        public string AccountType3 { get; private set; }

        public int AccountType4 { get; private set; }

        public int AccountType5 { get; private set; }

        public string Info { get; private set; }
    }
}