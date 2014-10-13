namespace DH.Helpdesk.Mobile.Areas.Licenses.Models.Applications
{
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Common;

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