namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class TabSettingModel
    {
        public TabSettingModel()
        {
        }

        public TabSettingModel(
            bool show,
            string caption)
        {
            Show = show;
            Caption = caption;
        }

        public bool Show { get; set; }


        [NotNull] [LocalizedRequired] public string Caption { get; set; }
    }
}