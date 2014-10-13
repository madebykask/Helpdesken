namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsViewModel
    {
        public StateFieldsViewModel()
        {
        }

        public StateFieldsViewModel(StateFieldsModel stateFieldsModel, SelectList states)
        {
            this.StateFieldsModel = stateFieldsModel;
            this.States = states;
        }

        [NotNull]
        public StateFieldsModel StateFieldsModel { get; set; }

        [NotNull]
        public SelectList States { get; set; }
    }
}