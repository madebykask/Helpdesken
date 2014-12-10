namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using System;
    using System.Collections.Generic;

    public sealed class AccountInformation
    {
        public AccountInformation(
            DateTime? startedDate,
            DateTime? finishDate,
            int eMailTypeId,
            bool homeDirectory,
            bool profile,
            string inventoryNumber,
            int? accountTypeId,
            List<int> accountType2,
            int? accountType3,
            int? accountType4,
            int? accountType5,
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

        public bool HomeDirectory { get; private set; }

        public bool Profile { get; private set; }

        public string InventoryNumber { get; private set; }

        public int? AccountTypeId { get; private set; }

        public List<int> AccountType2 { get; private set; }

        public int? AccountType3 { get; private set; }

        public int? AccountType4 { get; private set; }

        public int? AccountType5 { get; private set; }

        public string Info { get; private set; }

        public static AccountInformation CreateDefault()
        {
            return new AccountInformation(null, null, 0, false, false, null, null, null, null, null, null, null);
        }
    }
}