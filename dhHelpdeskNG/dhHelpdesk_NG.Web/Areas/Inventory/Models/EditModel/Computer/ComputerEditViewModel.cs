using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerEditViewModel : BaseViewEditWorkstationModel
    {
        public ComputerEditViewModel(int id, ComputerViewModel computerViewModel)
            : base(id)
        {
            ComputerViewModel = computerViewModel;
        }

        [NotNull]
        public ComputerViewModel ComputerViewModel { get; set; }

        public bool UserHasInventoryAdminPermission { get; set; }

        public override WorkstationEditTabs Tab
        {
            get
            {
                return WorkstationEditTabs.Workstation;
            }
        }
    }
}