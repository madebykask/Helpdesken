using DH.Helpdesk.Services.Services;// Anpassa om modulerna ligger på annat ställe
using Microsoft.Extensions.Configuration;
using Ninject;
using DH.Helpdesk.Services.Infrastructure.Email;
using DH.Helpdesk.CaseSolutionYearly.DI.Modules;
using Serilog;

namespace DH.Helpdesk.CaseSolutionYearly.Resolver
{
    public static class ServiceResolver
    {
        private static IKernel _kernel;

        /// <summary>
        /// Call once to initialize the resolver with configuration.
        /// </summary>
        public static void Initialize(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Helpdesk");

            _kernel = new StandardKernel(
                new ServiceModule(),
                new InfrastructureModule(),
                new DatabaseModule(connectionString) // Skickar in till dina bindings
            );
        }

        public static ICaseService GetCaseService() => _kernel.Get<ICaseService>();
        public static ICaseMailer GetCaseMailerService() => _kernel.Get<ICaseMailer>();
        public static ILogService GetCaseLogService() => _kernel.Get<ILogService>();
        public static IEmailService GetEmailService() => _kernel.Get<IEmailService>();
        public static IMailTemplateService GetTemplateService() => _kernel.Get<IMailTemplateService>();
        public static ICaseSolutionService GetCaseSolutionService() => _kernel.Get<ICaseSolutionService>();

        // Lägg till fler .Get<T>() om du behöver
    }
}
