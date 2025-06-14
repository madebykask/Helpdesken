﻿namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class MemoryFieldsSettingsModel
    {
        public MemoryFieldsSettingsModel()
        {
        }

        public MemoryFieldsSettingsModel(FieldSettingModel ramFieldSettingModel)
        {
            this.RAMFieldSettingModel = ramFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("RAM")]
        public FieldSettingModel RAMFieldSettingModel { get; set; }
    }
}