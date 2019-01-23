using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    public abstract class BaseEditWorkstationModel
    {
        public const string Hotfix = "Hotfix";

        protected BaseEditWorkstationModel(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public bool IsForDialog { get; set; }

        public string UserId { get; set; }

        public abstract WorkstationEditTabs Tab { get; }
    }

    public abstract class BaseViewEditWorkstationModel : BaseEditWorkstationModel
    {
        protected BaseViewEditWorkstationModel(int id) : base(id)
        {
        }

        [NotNull]
        public WorkstationTabsSettings TabSettings { get; set; }

    }
}