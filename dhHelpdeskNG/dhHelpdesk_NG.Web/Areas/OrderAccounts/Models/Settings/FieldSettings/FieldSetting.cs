namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class FieldSetting
    {
        public FieldSetting()
        {
        }

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

        public bool IsShowInDetails { get;  set; }

        public bool IsShowInList { get;  set; }

        public bool IsShowExternal { get;  set; }

        [LocalizedRequired]
        public string Caption { get;  set; }

        public string Help { get;  set; }

        public bool IsRequired { get;  set; }
    }
}
