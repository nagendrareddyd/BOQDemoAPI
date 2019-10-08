using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace BOQ.API.Configuration
{
    internal static partial class ServiceCollectionExtensions
    {
        public static void AddAndConfigureSwagger(this IServiceCollection services)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = $"{AppDomain.CurrentDomain.FriendlyName}.xml";

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "BOQ Conference Api", Version = "v1" });
                options.AddSecurityDefinition("Basic", new ApiKeyScheme()
                {
                    Description = "Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = "header",
                    Type = "basic"
                });
                options.DocumentFilter<SwaggerSecurityDocumentFilter>();
                options.IncludeXmlComments(Path.Combine(basePath, fileName));
            });
        }
    }
}
