﻿namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ContactInformationFieldsSettingsModel
    {
        public ContactInformationFieldsSettingsModel()
        {
        }

        public ContactInformationFieldsSettingsModel(FieldSettingModel userIdFieldSettingModel)
        {
            this.UserIdFieldSettingModel = userIdFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Användare ID")]
        public FieldSettingModel UserIdFieldSettingModel { get; set; }
    }
}