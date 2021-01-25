using APICore.Settings;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace APICore.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;
        private readonly ApiSettings apiSettings;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, ApiSettings apiSettings)
        {
            this.provider = provider;
            this.apiSettings = apiSettings;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = this.apiSettings.Title,
                Version = description.ApiVersion.ToString(),
                Description = this.apiSettings.Description
            };

            if (description.IsDeprecated)
            {
                info.Description += $" {this.apiSettings.ApiDeprecated}";
            }

            return info;
        }
    }
}
