using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.Repositories.Cases.Concrete;
using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;
using UnitOfWork = DH.Helpdesk.Dal.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace DH.Helpdesk.CaseSolutionYearly.Services
{
    public static class HelpdeskServiceExtensions
    {
        public static IServiceCollection AddHelpdeskServices(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseFactory>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("Helpdesk");
                return new DatabaseFactory(connectionString);
            });
            services.AddScoped<ICaseSolutionRepository, CaseSolutionRepository>();
            services.AddScoped<ICaseSolutionCategoryRepository, CaseSolutionCategoryRepository>();
            services.AddScoped<ICaseSolutionScheduleRepository, CaseSolutionScheduleRepository>();
            services.AddScoped<ICaseSolutionSettingRepository, CaseSolutionSettingRepository>();
            services.AddScoped<IFormRepository, FormRepository>();
            services.AddScoped<ILinkRepository, LinkRepository>();
            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICaseSolutionConditionRepository, CaseSolutionConditionRepository>();
            services.AddScoped<ICacheProvider, CacheProvider>();
            services.AddScoped<IWorkingGroupService, WorkingGroupService>();
            services.AddScoped<IComputerUserCategoryRepository, ComputerUserCategoryRepository>();
            //services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddScoped<ICaseSolutionLanguageRepository, CaseSolutionLanguageRepository>();
            services.AddScoped<ICaseSolutionCategoryLanguageRepository, CaseSolutionCategoryLanguageRepository>();
            services.AddScoped<ICaseSolutionService, CaseSolutionService>();
            services.AddScoped<ILinkGroupRepository, LinkGroupRepository>();


            return services;
        }
    }
}
