using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Web.Infrastructure.Configuration
{
    using System.Globalization;

    public interface IApplicationConfiguration
    {
        CultureInfo DefaultCulture { get; }
        string ApplicationId { get; }
        LoginMode LoginMode { get; }
    }
}