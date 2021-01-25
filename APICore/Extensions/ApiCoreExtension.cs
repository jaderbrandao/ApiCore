using APICore.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APICore.Extensions
{
    public static class ApiCoreExtension
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var apiSettings = new ApiSettings();
            configuration.Bind("ApiSettings", apiSettings);
            services.AddSingleton(apiSettings);


            services.AddMvcConfiguration(apiSettings);
        }
    }
}
