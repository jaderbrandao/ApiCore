using APICore.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace APICore.Extensions
{
    public static class APIVersioningExtension
    {
        public static void UseAPiVersioning(this IServiceCollection services, string applicationName)
        {
            services.AddApiVersioning(
            options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(
            options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";
                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo() { Title = $"{applicationName} {description.ApiVersion}", Version = description.ApiVersion.ToString() });

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var fileXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var pathXml = Path.Combine(AppContext.BaseDirectory, fileXml);
                options.IncludeXmlComments(pathXml);
                options.CustomSchemaIds(x => x.FullName);
                options.DescribeAllParametersInCamelCase();
            });
        }
    }
}
