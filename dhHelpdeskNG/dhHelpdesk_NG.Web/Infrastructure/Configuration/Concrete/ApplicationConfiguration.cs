namespace DH.Helpdesk.Web.Infrastructure.Configuration.Concrete
{
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Web;

    internal sealed class ApplicationConfiguration : IApplicationConfiguration
    {
        private CultureInfo defaultCulture;

        public CultureInfo DefaultCulture
        {
            get
            {
                if (this.defaultCulture == null)
                {
                    var fromConfig = ConfigurationManager.AppSettings["Application.DefaultCulture"];
                    if (!string.IsNullOrEmpty(fromConfig))
                    {
                        this.defaultCulture = new CultureInfo(fromConfig);
                        return this.defaultCulture;
                    }

                    if (this.defaultCulture == null)
                    {
                        if (HttpContext.Current.Request.UserLanguages != null)
                        {
                            var userLaunguage = HttpContext.Current.Request.UserLanguages.FirstOrDefault();
                            if (!string.IsNullOrEmpty(userLaunguage))
                            {
                                this.defaultCulture = new CultureInfo(userLaunguage);
                                return this.defaultCulture;
                            }
                        }                        
                    }

                    if (this.defaultCulture == null)
                    {
                        this.defaultCulture = new CultureInfo(Services.Infrastructure.ApplicationDefaultParameters.Culture);                        
                    }
                }

                return this.defaultCulture;
            }
        }
    }
}