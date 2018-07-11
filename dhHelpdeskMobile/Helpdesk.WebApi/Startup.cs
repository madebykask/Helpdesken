using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Helpdesk.WebApi.Config;
using Helpdesk.WebApi.Config.IdentityServer;
using IdentityServer4.AccessTokenValidation;

namespace Helpdesk.WebApi
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        private readonly ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DependencyInjection.ConfigureDi(services, _loggerFactory);

            services.AddIdentityServer(options => { options.UserInteraction.LoginUrl = "http://localhost:8111/login.html"; })
                .AddDeveloperSigningCredential()
                //.AddSigningCredential(cert)
                .AddInMemoryApiResources(Settings.GetApiResources())
                //.AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Settings.GetIdentityResources())
                .AddTestUsers(Settings.GetUsers());

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            services.AddAuthentication()
                //.AddCookie("dhhelpdesk")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority =
                        Settings.Authorization
                            .AuthorityName; //The Authority indicates that we are trusting IdentityServer
                    options.RequireHttpsMetadata = true;

                    options.ApiName = Settings.Authorization.Scopes.ServerApiScopeName;
                    options.SupportedTokens = SupportedTokens.Both;
                    options.SaveToken = true;
                    //Caching will be used if uncommented and services has IDistributedCache registered
                    //options.EnableCaching = true;
                    //options.CacheDuration = TimeSpan.FromMinutes(10); // that's the default
                });
                //.AddOpenIdConnect("oidc", "OpenID Connect", options =>
                //{

                //});

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("all", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
                //options.AddPolicy("default", policy =>
                //{
                //    policy.WithOrigins("https://localhost:449") // TODO: update to get list of allowed resources from config
                //        .AllowAnyHeader()
                //        .AllowAnyMethod();
                //});
            });

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("dataEventRecordsAdmin", policyAdmin =>
                //{
                //    policyAdmin.RequireClaim("role", "dataEventRecords.admin");
                //});
                //options.AddPolicy("admin", policyAdmin =>
                //{
                //    policyAdmin.RequireClaim("role", "admin");
                //});
                //options.AddPolicy("dataEventRecordsUser", policyUser =>
                //{
                //    policyUser.RequireClaim("role", "dataEventRecords.user");
                //});
                //options.AddPolicy("dataEventRecords", policyUser =>
                //{
                //    policyUser.RequireClaim("scope", "dataEventRecords");
                //});
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://asp.net/core",
                        Detail = "Please refer to the errors property for additional details."
                    };
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            services.AddMvc(opt =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireScope(Settings.Authorization.Scopes.ServerApiScopeName).Build();
                    //opt.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                //.AddJsonOptions(options => {
                //    // send back a ISO date
                //    var settings = options.SerializerSettings;
                //    settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                //    // dont mess with case of properties
                //    var resolver = options.SerializerSettings.ContractResolver as DefaultContractResolver;
                //    resolver.NamingStrategy = null;
                //});

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("all");
            app.UseIdentityServer();
            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
