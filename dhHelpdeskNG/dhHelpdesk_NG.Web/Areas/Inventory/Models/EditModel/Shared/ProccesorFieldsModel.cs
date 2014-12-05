namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProccesorFieldsModel
    {
        public ProccesorFieldsModel()
        {
        }

        public ProccesorFieldsModel(ConfigurableFieldModel<int?> proccesorId)
        {
            this.ProccesorId = proccesorId;
        }

        [NotNull]
        public ConfigurableFieldModel<int?> ProccesorId { get; set; }
    }
}