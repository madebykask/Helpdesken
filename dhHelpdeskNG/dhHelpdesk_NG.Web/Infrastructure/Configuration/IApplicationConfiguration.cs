namespace DH.Helpdesk.Web.Infrastructure.Configuration
{
    using System.Globalization;

    public interface IApplicationConfiguration
    {
        CultureInfo DefaultCulture { get; }
    }
}