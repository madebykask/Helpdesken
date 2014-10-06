namespace DH.Helpdesk.Mobile.Infrastructure.Configuration.Concrete
{
    internal sealed class Configuration : IConfiguration
    {
        public Configuration(IApplicationConfiguration application)
        {
            this.Application = application;
        }

        public IApplicationConfiguration Application { get; private set; }
    }
}