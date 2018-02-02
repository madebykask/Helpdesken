
namespace DH.Helpdesk.Web.Infrastructure.Configuration.Concrete
{
    internal sealed class Configuration : IConfiguration
    {
        public Configuration(IApplicationConfiguration application, IAdfsConfiguration adfsConfiguration)
        {
            Application = application;
            Adfs = adfsConfiguration;
        }

        public IApplicationConfiguration Application { get; private set; }

        public IAdfsConfiguration Adfs { get; private set; }
    }
}