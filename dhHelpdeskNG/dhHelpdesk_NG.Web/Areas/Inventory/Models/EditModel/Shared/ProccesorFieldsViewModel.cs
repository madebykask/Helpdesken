namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Shared
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProccesorFieldsViewModel
    {
        public ProccesorFieldsViewModel()
        {
        }

        public ProccesorFieldsViewModel(
            ProccesorFieldsModel proccesorFieldsModel,
            SelectList proccessors)
        {
            this.ProccesorFieldsModel = proccesorFieldsModel;
            this.Proccessors = proccessors;
        }

        [NotNull]
        public ProccesorFieldsModel ProccesorFieldsModel { get; set; }

        [NotNull]
        public SelectList Proccessors { get; set; }
    }
}