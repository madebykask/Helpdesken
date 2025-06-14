﻿using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class LogFieldSettingsModel
    {
        public LogFieldSettingsModel()
        {            
        }

        public LogFieldSettingsModel(TextFieldSettingsModel log)
        {
            this.Log = log;
        }

        [NotNull]
        [LocalizedDisplay(OrderLabels.Log)]
        public TextFieldSettingsModel Log { get; set; }
    }
}