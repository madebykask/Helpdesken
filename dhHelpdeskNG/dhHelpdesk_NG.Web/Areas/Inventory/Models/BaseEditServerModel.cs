namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    public abstract class BaseEditServerModel
    {
        public const string Hotfix = "Hotfix";

        protected BaseEditServerModel(int id)
        {
            this.Id = id;
        }

        public int Id { get; set; }

        public bool IsForDialog { get; set; }

        public abstract ServerEditTabs Tab { get; }
    }
}