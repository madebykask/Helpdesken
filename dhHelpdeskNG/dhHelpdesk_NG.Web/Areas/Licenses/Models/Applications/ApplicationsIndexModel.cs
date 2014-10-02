namespace DH.Helpdesk.Web.Areas.Licenses.Models.Applications
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;

    public sealed class ApplicationsIndexModel : BaseIndexModel
    {
        public override IndexModelType Type
        {
            get
            {
                return IndexModelType.Applications;
            }
        }

        public bool OnlyConnected { get; set; }

        public ApplicationsFilterModel GetFilter()
        {
            return new ApplicationsFilterModel(this.OnlyConnected);
        }
    }
}