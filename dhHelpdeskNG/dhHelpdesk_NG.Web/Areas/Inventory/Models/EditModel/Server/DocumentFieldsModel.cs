namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DocumentFieldsModel
    {
        public DocumentFieldsModel()
        {
        }

        public DocumentFieldsModel(ConfigurableFieldModel<string> document)
        {
            this.Document = document;
        }

        [NotNull]
        public ConfigurableFieldModel<string> Document { get; set; }
    }
}