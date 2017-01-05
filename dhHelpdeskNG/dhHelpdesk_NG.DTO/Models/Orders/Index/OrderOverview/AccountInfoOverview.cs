using System;
using DH.Helpdesk.Domain.Orders;

namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public class AccountInfoOverview
    {
        public AccountInfoOverview(DateTime? startedDate, DateTime? finishDate, EMailTypes? eMailTypeId, bool homeDirectory, bool profile, string inventoryNumber, string accountTypeId, string accountTypeId2, string accountTypeId3, string accountTypeId4, string accountTypeId5, string info)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            AccountTypeId = accountTypeId;
            AccountTypeId2 = accountTypeId2;
            AccountTypeId3 = accountTypeId3;
            AccountTypeId4 = accountTypeId4;
            AccountTypeId5 = accountTypeId5;
            Info = info;
        }

        public DateTime? StartedDate { get; private set; }
        public DateTime? FinishDate { get; private set; }
        public EMailTypes? EMailTypeId { get; private set; }
        public bool HomeDirectory { get; private set; }
        public bool Profile { get; private set; }
        public string InventoryNumber { get; private set; }
        public string AccountTypeId { get; private set; }
        public string AccountTypeId2 { get; private set; }
        public string AccountTypeId3 { get; private set; }
        public string AccountTypeId4 { get; private set; }
        public string AccountTypeId5 { get; private set; }
        public string Info { get; private set; }
    }
}
