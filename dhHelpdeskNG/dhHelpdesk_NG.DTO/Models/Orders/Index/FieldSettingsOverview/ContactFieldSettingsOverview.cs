using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Shared.Output;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    public class ContactFieldSettingsOverview
    {
        public ContactFieldSettingsOverview(FieldOverviewSetting id,
            FieldOverviewSetting name,
            FieldOverviewSetting phone,
            FieldOverviewSetting eMail)
        {
            Id = id;
            Name = name;
            Phone = phone;
            EMail = eMail;
        }

        [NotNull]
        public FieldOverviewSetting Id { get; private set; }

        [NotNull]
        public FieldOverviewSetting Name { get; private set; }

        [NotNull]
        public FieldOverviewSetting Phone { get; private set; }

        [NotNull]
        public FieldOverviewSetting EMail { get; private set; }

    }
}
