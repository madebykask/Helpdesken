using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    public class AccountInfoFieldSettings
    {
        public AccountInfoFieldSettings(TextFieldSettings startedDate,
            TextFieldSettings finishDate, 
            TextFieldSettings eMailTypeId, 
            TextFieldSettings homeDirectory, 
            TextFieldSettings profile, 
            TextFieldSettings inventoryNumber, 
            TextFieldSettings info,
            OrderFieldTypeSettings accountType,
            OrderFieldTypeSettings accountType2,
            OrderFieldTypeSettings accountType3,
            OrderFieldTypeSettings accountType4,
            OrderFieldTypeSettings accountType5)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
            Info = info;
            AccountType = accountType;
            AccountType2 = accountType2;
            AccountType3 = accountType3;
            AccountType4 = accountType4;
            AccountType5 = accountType5;
        }

        public TextFieldSettings StartedDate { get; private set; }

        public TextFieldSettings FinishDate { get; private set; }

        public TextFieldSettings EMailTypeId { get; private set; }

        public TextFieldSettings HomeDirectory { get; private set; }

        public TextFieldSettings Profile { get; private set; }

        public TextFieldSettings InventoryNumber { get; private set; }

        public OrderFieldTypeSettings AccountType { get; private set; }

        public OrderFieldTypeSettings AccountType2 { get; private set; }

        public OrderFieldTypeSettings AccountType3 { get; private set; }

        public OrderFieldTypeSettings AccountType4 { get; private set; }

        public OrderFieldTypeSettings AccountType5 { get; private set; }

        public TextFieldSettings Info { get; private set; }
    }
}
