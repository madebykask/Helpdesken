using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain.Orders;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    public class AccountInfoHistoryFields
    {
        public AccountInfoHistoryFields(DateTime? startedDate,
            DateTime? finishDate,
            EMailTypes? eMailTypeId,
            bool? homeDirectory,
            bool? profile,
            string inventoryNumber,
            string info,
            int? accountTypeId,
            string accountTypeIdName,
            int? accountTypeId3,
            string accountTypeId3Name,
            int? accountTypeId4,
            string accountTypeId4Name,
            int? accountTypeId5,
            string accountTypeId5Name)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
            AccountTypeId = accountTypeId;
            AccountTypeIdName = accountTypeIdName;
            AccountTypeId3 = accountTypeId3;
            AccountTypeId3Name = accountTypeId3Name;
            AccountTypeId4 = accountTypeId4;
            AccountTypeId4Name = accountTypeId4Name;
            AccountTypeId5 = accountTypeId5;
            AccountTypeId5Name = accountTypeId5Name;
        }

        public DateTime? StartedDate { get; private set; }

        public DateTime? FinishDate { get; private set; }

        public EMailTypes? EMailTypeId { get; private set; }

        public bool? HomeDirectory { get; private set; }

        public bool? Profile { get; private set; }

        public string InventoryNumber { get; private set; }

        public string Info { get; private set; }

        public int? AccountTypeId { get; private set; }

        public string AccountTypeIdName { get; private set; }

        //public List<int> AccountTypeId2 { get; private set; }

        public int? AccountTypeId3 { get; private set; }

        public string AccountTypeId3Name { get; private set; }

        public int? AccountTypeId4 { get; private set; }

        public string AccountTypeId4Name { get; private set; }

        public int? AccountTypeId5 { get; private set; }

        public string AccountTypeId5Name { get; private set; }
    }
}
