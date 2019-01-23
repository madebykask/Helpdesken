using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    public class DocumentFieldsSettings
    {
        public DocumentFieldsSettings(ModelEditFieldSetting documentFieldSetting)
        {
            DocumentFieldSetting = documentFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting DocumentFieldSetting { get; set; }
    }
}