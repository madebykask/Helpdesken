namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DocumentFieldsSettings
    {
        public DocumentFieldsSettings(ModelEditFieldSetting documentFieldSetting)
        {
            this.DocumentFieldSetting = documentFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting DocumentFieldSetting { get; set; }
    }
}