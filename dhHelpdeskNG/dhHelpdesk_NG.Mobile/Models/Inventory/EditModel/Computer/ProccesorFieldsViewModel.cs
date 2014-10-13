namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProccesorFieldsViewModel
    {
        public ProccesorFieldsViewModel()
        {
        }

        public ProccesorFieldsViewModel(ProccesorFieldsModel proccesorFieldsModel, SelectList proccessors)
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