﻿namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class SoundFieldsSettingsModel
    {
        public SoundFieldsSettingsModel()
        {
        }

        public SoundFieldsSettingsModel(FieldSettingModel soundCardFieldSettingModel)
        {
            this.SoundCardFieldSettingModel = soundCardFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Ljudkort")]
        public FieldSettingModel SoundCardFieldSettingModel { get; set; }
    }
}