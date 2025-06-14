﻿namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ProgramFieldSettings
    {
        public ProgramFieldSettings()
        {
        }

        public ProgramFieldSettings(
            FieldSetting programs,
            FieldSetting infoProduct)
        {
            this.Programs = programs;
            this.InfoProduct = infoProduct;
        }

        [LocalizedDisplay("Programvaror")]
        public FieldSetting Programs { get;  set; }

        [LocalizedDisplay("Övrigt program")]
        public FieldSetting InfoProduct { get;  set; }
    }
}