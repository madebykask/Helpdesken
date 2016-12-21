using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    public class AccountInfoFieldSettings
    {
        public AccountInfoFieldSettings(TextFieldSettings startedDate, TextFieldSettings finishDate, TextFieldSettings eMailTypeId, TextFieldSettings homeDirectory, TextFieldSettings profile, TextFieldSettings inventoryNumber, TextFieldSettings info)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
        }

        public TextFieldSettings StartedDate { get; private set; }

        public TextFieldSettings FinishDate { get; private set; }

        public TextFieldSettings EMailTypeId { get; private set; }

        public TextFieldSettings HomeDirectory { get; private set; }

        public TextFieldSettings Profile { get; private set; }

        public TextFieldSettings InventoryNumber { get; private set; }

        //public FieldSetting AccountTypeId { get; private set; }

        //public FieldSetting AccountType2 { get; private set; }

        //public FieldSetting AccountType3 { get; private set; }

        //public FieldSetting AccountType4 { get; private set; }

        //public FieldSetting AccountType5 { get; private set; }

        public TextFieldSettings Info { get; private set; }
    }
}
