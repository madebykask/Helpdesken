using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    public class AccountInfoEditSettings
    {
        public AccountInfoEditSettings(FieldEditSettings startedDate, FieldEditSettings finishDate, FieldEditSettings eMailTypeId, FieldEditSettings homeDirectory, FieldEditSettings profile, TextFieldEditSettings inventoryNumber, TextFieldEditSettings info, FieldEditSettings accountTypeId, FieldEditSettings accountTypeId2, FieldEditSettings accountTypeId3, FieldEditSettings accountTypeId4, FieldEditSettings accountTypeId5)
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

        public FieldEditSettings StartedDate { get; private set; }

        public FieldEditSettings FinishDate { get; private set; }

        public FieldEditSettings EMailTypeId { get; private set; }

        public FieldEditSettings HomeDirectory { get; private set; }

        public FieldEditSettings Profile { get; private set; }

        public TextFieldEditSettings InventoryNumber { get; private set; }

        public TextFieldEditSettings Info { get; private set; }

        public FieldEditSettings AccountTypeId { get; private set; }

        public FieldEditSettings AccountTypeId2 { get; private set; }

        public FieldEditSettings AccountTypeId3 { get; private set; }

        public FieldEditSettings AccountTypeId4 { get; private set; }

        public FieldEditSettings AccountTypeId5 { get; private set; }

    }
}
