namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsViewModel
    {
        public StateFieldsViewModel()
        {
        }

        public StateFieldsViewModel(StateFieldsModel stateFieldsModel, ConfigurableFieldModel<SelectList> states)
        {
            this.StateFieldsModel = stateFieldsModel;
            this.States = states;
        }

        [NotNull]
        public StateFieldsModel StateFieldsModel { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> States { get; set; }
    }
}