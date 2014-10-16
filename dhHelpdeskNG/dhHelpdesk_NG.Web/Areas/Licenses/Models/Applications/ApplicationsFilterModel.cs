namespace DH.Helpdesk.Web.Areas.Licenses.Models.Applications
{
    public sealed class ApplicationsFilterModel
    {
        public ApplicationsFilterModel(string name, bool onlyConnected)
        {
            this.Name = name;
            this.OnlyConnected = onlyConnected;
        }

        private ApplicationsFilterModel()
        {
        }

        public string Name { get; private set; }

        public bool OnlyConnected { get; private set; }

        public static ApplicationsFilterModel CreateDefault()
        {
            return new ApplicationsFilterModel();
        }
    }
}