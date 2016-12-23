using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    public class AccountInfoEditFields
    {
        public AccountInfoEditFields(DateTime? startedDate, DateTime? finishDate, int eMailTypeId, bool homeDirectory, bool profile, string inventoryNumber, string info)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
        }

        public DateTime? StartedDate { get; private set; }

        public DateTime? FinishDate { get; private set; }

        public int EMailTypeId { get; private set; }

        public bool HomeDirectory { get; private set; }

        public bool Profile { get; private set; }

        public string InventoryNumber { get; private set; }

        //public FieldSetting AccountTypeId { get; private set; }

        //public FieldSetting AccountType2 { get; private set; }

        //public FieldSetting AccountType3 { get; private set; }

        //public FieldSetting AccountType4 { get; private set; }

        //public FieldSetting AccountType5 { get; private set; }

        public string Info { get; private set; }
    }
}
