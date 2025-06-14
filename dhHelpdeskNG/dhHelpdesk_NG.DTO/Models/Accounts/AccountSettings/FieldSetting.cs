﻿namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSetting
    {
        public FieldSetting(
            bool isShowInDetails,
            bool isShowInList,
            bool isShowExternal,
            string caption,
            string help,
            bool isRequired)
        {
            this.IsShowInDetails = isShowInDetails;
            this.IsShowInList = isShowInList;
            this.IsShowExternal = isShowExternal;
            this.Caption = caption;
            this.Help = help;
            this.IsRequired = isRequired;
        }

        public bool IsShowInDetails { get; private set; }

        public bool IsShowInList { get; private set; }

        public bool IsShowExternal { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public string Help { get; private set; }

        public bool IsRequired { get; private set; }
    }
}
