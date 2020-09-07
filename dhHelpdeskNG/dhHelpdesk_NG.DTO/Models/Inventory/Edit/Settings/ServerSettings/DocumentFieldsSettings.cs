namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DocumentFieldsSettings
    {
        public DocumentFieldsSettings(FieldSetting docuemntFieldSetting)
        {
            this.DocuemntFieldSetting = docuemntFieldSetting;
        }

        [NotNull]
        public FieldSetting DocuemntFieldSetting { get; set; }
    }
}