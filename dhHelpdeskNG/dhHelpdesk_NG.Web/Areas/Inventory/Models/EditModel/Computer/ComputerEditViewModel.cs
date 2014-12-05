namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerEditViewModel : BaseEditWorkstationModel
    {
        public ComputerEditViewModel(int id, ComputerViewModel computerViewModel)
            : base(id)
        {
            this.ComputerViewModel = computerViewModel;
        }

        [NotNull]
        public ComputerViewModel ComputerViewModel { get; set; }

        public override WorkstationEditTabs Tab
        {
            get
            {
                return WorkstationEditTabs.Workstation;
            }
        }
    }
}