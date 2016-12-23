using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    public class AccountInfoEditSettings
    {
        public AccountInfoEditSettings(FieldEditSettings startedDate, FieldEditSettings finishDate, FieldEditSettings eMailTypeId, FieldEditSettings homeDirectory, FieldEditSettings profile, TextFieldEditSettings inventoryNumber, TextFieldEditSettings info)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
        }

        public FieldEditSettings StartedDate { get; private set; }

        public FieldEditSettings FinishDate { get; private set; }

        public FieldEditSettings EMailTypeId { get; private set; }

        public FieldEditSettings HomeDirectory { get; private set; }

        public FieldEditSettings Profile { get; private set; }

        public TextFieldEditSettings InventoryNumber { get; private set; }

        //public FieldSetting AccountTypeId { get; private set; }

        //public FieldSetting AccountType2 { get; private set; }

        //public FieldSetting AccountType3 { get; private set; }

        //public FieldSetting AccountType4 { get; private set; }

        //public FieldSetting AccountType5 { get; private set; }

        public TextFieldEditSettings Info { get; private set; }
    }
}
