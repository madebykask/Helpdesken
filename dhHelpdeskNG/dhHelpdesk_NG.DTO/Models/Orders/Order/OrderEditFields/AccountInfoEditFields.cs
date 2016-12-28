using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    public class AccountInfoEditFields
    {
        public AccountInfoEditFields(DateTime? startedDate, DateTime? finishDate, int eMailTypeId, bool homeDirectory, bool profile, string inventoryNumber, string info, int? accountTypeId, List<int> accountTypeId2, int? accountTypeId3, int? accountTypeId4, int? accountTypeId5)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
            AccountTypeId = accountTypeId;
            AccountTypeId2 = accountTypeId2;
            AccountTypeId3 = accountTypeId3;
            AccountTypeId4 = accountTypeId4;
            AccountTypeId5 = accountTypeId5;
        }

        public DateTime? StartedDate { get; private set; }

        public DateTime? FinishDate { get; private set; }

        public int EMailTypeId { get; private set; }

        public bool HomeDirectory { get; private set; }

        public bool Profile { get; private set; }

        public string InventoryNumber { get; private set; }

        public string Info { get; private set; }

        public int? AccountTypeId { get; private set; }

        public List<int> AccountTypeId2 { get; private set; }

        public int? AccountTypeId3 { get; private set; }

        public int? AccountTypeId4 { get; private set; }

        public int? AccountTypeId5 { get; private set; }

    }
}
