using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Configuration
{
    public interface IConfiguration
    {
        IApplicationConfiguration Application { get; }
        IAdfsConfiguration Adfs { get; }
    }
}