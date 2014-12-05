namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    public abstract class BaseEditWorkstationModel
    {
        public const string Hotfix = "Hotfix";

        protected BaseEditWorkstationModel(int id)
        {
            this.Id = id;
        }

        public int Id { get; set; }

        public abstract WorkstationEditTabs Tab { get; }
    }
}