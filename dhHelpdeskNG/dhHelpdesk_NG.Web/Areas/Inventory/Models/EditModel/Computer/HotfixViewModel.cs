namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class HotfixViewModel : BaseViewEditWorkstationModel
    {
        public HotfixViewModel(int id, List<SoftwareOverview> overviews)
            : base(id)
        {
            this.Overviews = overviews.Where(x => x.Name.Contains(Hotfix)).Select(x => x.Name).OrderBy(x => x).ToList();
        }

        [NotNull]
        public List<string> Overviews { get; set; }

        public override WorkstationEditTabs Tab
        {
            get
            {
                return WorkstationEditTabs.Hotfix;
            }
        }
    }
}