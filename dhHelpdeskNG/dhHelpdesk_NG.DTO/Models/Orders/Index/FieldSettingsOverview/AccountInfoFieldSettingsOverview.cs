using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Shared.Output;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    public class AccountInfoFieldSettingsOverview
    {
        public AccountInfoFieldSettingsOverview(FieldOverviewSetting startedDate,
            FieldOverviewSetting finishDate,
            FieldOverviewSetting eMailTypeId,
            FieldOverviewSetting homeDirectory,
            FieldOverviewSetting profile,
            FieldOverviewSetting inventoryNumber,
            FieldOverviewSetting accountTypeId,
            FieldOverviewSetting accountTypeId2,
            FieldOverviewSetting accountTypeId3,
            FieldOverviewSetting accountTypeId4,
            FieldOverviewSetting accountTypeId5,
            FieldOverviewSetting info)
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

        [NotNull]
        public FieldOverviewSetting StartedDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting FinishDate { get; private set; }


        [NotNull]
        public FieldOverviewSetting EMailTypeId { get; private set; }


        [NotNull]
        public FieldOverviewSetting HomeDirectory { get; private set; }


        [NotNull]
        public FieldOverviewSetting Profile { get; private set; }


        [NotNull]
        public FieldOverviewSetting InventoryNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountTypeId { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountTypeId2 { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountTypeId3 { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountTypeId4 { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountTypeId5 { get; private set; }


        [NotNull]
        public FieldOverviewSetting Info { get; private set; }
    }
}
