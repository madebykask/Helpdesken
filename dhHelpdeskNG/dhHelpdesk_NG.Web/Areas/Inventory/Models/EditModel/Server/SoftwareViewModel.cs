namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SoftwareViewModel : BaseEditServerModel
    {
        public SoftwareViewModel(int id, List<SoftwareOverview> overviews)
            : base(id)
        {
            this.Overviews = overviews.Where(x => !x.Name.Contains(Hotfix)).OrderBy(x => x.Name).ToList();
        }

        [NotNull]
        public List<SoftwareOverview> Overviews { get; set; }

        public override ServerEditTabs Tab
        {
            get
            {
                return ServerEditTabs.Programs;
            }
        }


    }
}