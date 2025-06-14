﻿namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class StorageFieldsSettingsModel
    {
        public StorageFieldsSettingsModel()
        {
        }

        public StorageFieldsSettingsModel(FieldSettingModel capasityFieldSettingModel)
        {
            this.CapasityFieldSettingModel = capasityFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Kapacitet")]
        public FieldSettingModel CapasityFieldSettingModel { get; set; }
    }
}