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
        public AccountInfoFieldSettingsOverview(FieldOverviewSetting startedDate, FieldOverviewSetting finishDate, FieldOverviewSetting eMailTypeId, FieldOverviewSetting homeDirectory, FieldOverviewSetting profile, FieldOverviewSetting inventoryNumber, FieldOverviewSetting info)
        {
            StartedDate = startedDate;
            FinishDate = finishDate;
            EMailTypeId = eMailTypeId;
            HomeDirectory = homeDirectory;
            Profile = profile;
            InventoryNumber = inventoryNumber;
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
        public FieldOverviewSetting Info { get; private set; }
    }
}
