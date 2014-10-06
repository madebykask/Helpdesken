namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class WorkstationFieldsViewModel
    {
        public WorkstationFieldsViewModel()
        {
        }

        public WorkstationFieldsViewModel(
            WorkstationFieldsModel workstationFieldsModel,
            SelectList computerModels,
            SelectList computerTypes)
        {
            this.WorkstationFieldsModel = workstationFieldsModel;
            this.ComputerModels = computerModels;
            this.ComputerTypes = computerTypes;
        }

        [NotNull]
        public WorkstationFieldsModel WorkstationFieldsModel { get; set; }

        [NotNull]
        public SelectList ComputerModels { get; set; }

        [NotNull]
        public SelectList ComputerTypes { get; set; }
    }
}