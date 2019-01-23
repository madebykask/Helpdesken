namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SoftwareViewModel : BaseViewEditWorkstationModel
    {
        public SoftwareViewModel(int id, List<SoftwareOverview> overviews)
            : base(id)
        {
            this.Overviews = overviews.Where(x => !x.Name.Contains(Hotfix)).OrderBy(x => x.Name).ToList();
        }

        [NotNull]
        public List<SoftwareOverview> Overviews { get; set; }

        public override WorkstationEditTabs Tab
        {
            get
            {
                return WorkstationEditTabs.Programs;
            }
        }


    }
}