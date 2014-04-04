namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class WorkstationFieldsViewModel
    {
        public WorkstationFieldsViewModel(
            WorkstationFieldsModel workstationFieldsModel,
            ConfigurableFieldModel<SelectList> computerModels,
            ConfigurableFieldModel<SelectList> computerTypes)
        {
            this.WorkstationFieldsModel = workstationFieldsModel;
            this.ComputerModels = computerModels;
            this.ComputerTypes = computerTypes;
        }

        [NotNull]
        public WorkstationFieldsModel WorkstationFieldsModel { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ComputerModels { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ComputerTypes { get; set; }
    }
}