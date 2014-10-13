namespace DH.Helpdesk.Web.Areas.Licenses.Models.Applications
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;

    public sealed class ApplicationsIndexModel : BaseIndexModel
    {
        public ApplicationsIndexModel(bool onlyConnected)
        {
            this.OnlyConnected = onlyConnected;
        }

        public ApplicationsIndexModel()
        {            
        }

        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Applications;
            }
        }

        public bool OnlyConnected { get; set; }

        public ApplicationsFilterModel GetFilter()
        {
            return new ApplicationsFilterModel(this.OnlyConnected);
        }
    }
}