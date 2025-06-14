﻿namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class FieldSettingModel
    {
        public FieldSettingModel()
        {
        }

        public FieldSettingModel(
            bool showInDetails,
            bool showInList,
            string caption,
            bool isRequired)
        {
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.Caption = caption;
            this.IsRequired = isRequired;
        }

        public bool ShowInDetails { get; set; }

        public bool ShowInList { get; set; }

        [NotNull]
        [LocalizedRequired]
        public string Caption { get; set; }

        public bool IsRequired { get; set; }
    }
}