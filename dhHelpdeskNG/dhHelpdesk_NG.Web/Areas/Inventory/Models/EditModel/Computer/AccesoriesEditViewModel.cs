namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Inventory.Models;

    public class AccesoriesEditViewModel : BaseEditWorkstationModel
    {
        public AccesoriesEditViewModel(int id, AccesoriesViewModel accesoriesViewModel)
            : base(id)
        {
            this.AccesoriesViewModel = accesoriesViewModel;
        }

        [NotNull]
        public AccesoriesViewModel AccesoriesViewModel { get; set; }

        public override WorkstationEditTabs Tab
        {
            get
            {
                return WorkstationEditTabs.Accessories;
            }
        }
    }
}