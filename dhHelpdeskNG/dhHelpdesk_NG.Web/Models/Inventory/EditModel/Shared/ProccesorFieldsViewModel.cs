namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProccesorFieldsViewModel
    {
        public ProccesorFieldsViewModel(ProccesorFieldsModel proccesorFieldsModel, ConfigurableFieldModel<SelectList> proccessors)
        {
            this.ProccesorFieldsModel = proccesorFieldsModel;
            this.Proccessors = proccessors;
        }

        [NotNull]
        public ProccesorFieldsModel ProccesorFieldsModel { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Proccessors { get; set; }
    }
}