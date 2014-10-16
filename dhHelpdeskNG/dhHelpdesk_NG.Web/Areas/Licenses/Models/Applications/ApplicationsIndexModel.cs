namespace DH.Helpdesk.Web.Areas.Licenses.Models.Applications
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;

    public sealed class ApplicationsIndexModel : BaseIndexModel
    {
        public ApplicationsIndexModel(
            string name,
            bool onlyConnected)
        {
            this.Name = name;
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

        public string Name { get; set; }

        public bool OnlyConnected { get; set; }

        public ApplicationsFilterModel GetFilter()
        {
            return new ApplicationsFilterModel(this.Name, this.OnlyConnected);
        }
    }
}