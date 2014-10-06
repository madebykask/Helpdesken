namespace DH.Helpdesk.Mobile.Areas.Licenses.Models.Applications
{
    public sealed class ApplicationsFilterModel
    {
        public ApplicationsFilterModel(bool onlyConnected)
        {
            this.OnlyConnected = onlyConnected;
        }

        private ApplicationsFilterModel()
        {
        }

        public bool OnlyConnected { get; private set; }

        public static ApplicationsFilterModel CreateDefault()
        {
            return new ApplicationsFilterModel();
        }
    }
}