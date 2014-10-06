namespace DH.Helpdesk.Mobile.Infrastructure.Configuration
{
    using System.Globalization;

    public interface IApplicationConfiguration
    {
        CultureInfo DefaultCulture { get; }

        string ApplicationId { get; }
    }
}