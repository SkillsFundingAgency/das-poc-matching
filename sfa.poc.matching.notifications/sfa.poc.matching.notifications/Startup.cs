using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notify.Client;
using Notify.Interfaces;
using sfa.poc.matching.notifications.Application.Data;
using sfa.poc.matching.notifications.Application.Interfaces;
using sfa.poc.matching.notifications.Application.Services;
using sfa.poc.matching.notifications.Application.Configuration;
using sfa.poc.matching.notifications.Services;
using SFA.DAS.Notifications.Api.Client.Configuration;

namespace sfa.poc.matching.notifications
{
    public class Startup
    {
        public IMatchingConfiguration Configuration { get; set; }
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _logger = logger;

            _logger.LogInformation("In startup constructor.  Before GetConfig");

            Configuration = ConfigurationService.GetConfig(configuration["EnvironmentName"],
                configuration["ConfigurationStorageConnectionString"],
                configuration["Version"],
                configuration["ServiceName"]).Result;

            _logger.LogInformation("In startup constructor.  After GetConfig");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IMatchingConfiguration>(provider => Configuration);
            services.AddTransient<NotificationsApiClientConfiguration>(provider => Configuration.NotificationsApiClientConfiguration);

            services.AddTransient<INotificationClient>(provider => new NotificationClient(Configuration.GovNotifyApiKey));

            services.AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();
            //services.AddTransient<IEmailService, DasNotifyEmailService>();
            services.AddTransient<IEmailService, GovNotifyEmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
