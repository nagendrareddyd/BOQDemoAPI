using BOQ.API.Configuration;
using BOQ.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BOQ.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var basicAuthSection = Configuration.GetSection(nameof(BasicAuthSettings));
            services.Configure<BasicAuthSettings>(basicAuthSection);
            var demoConferenceAPISettingsSection = Configuration.GetSection(nameof(DemoConferenceAPISettings));
            services.Configure<DemoConferenceAPISettings>(demoConferenceAPISettingsSection);
            var demoConferenceAPISettings = demoConferenceAPISettingsSection.Get<DemoConferenceAPISettings>();

            services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
            {
                client.DefaultRequestHeaders.Add(demoConferenceAPISettings.SubscriptionKeyName, demoConferenceAPISettings.SubscriptionKeyValue);
            });

            services.AddSingleton<IDemoConferenceService, DemoConferenceService>();

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddAndConfigureSwagger();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BOQ Conference Api");
            });
        }
    }
}
