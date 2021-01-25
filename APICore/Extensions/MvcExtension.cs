using APICore.Dto;
using APICore.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text.Json;

namespace APICore.Extensions
{
    public static class MvcExtension
    {
        public static void AddMvcConfiguration(this IServiceCollection services, ApiSettings apisettings)
        {
            services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                o.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var listPropertiesWithProblem  = context.ModelState.Where(a => a.Value.Errors.Any()).Select(x => x.Key);
                    var propertiesWithProblem  = string.Join(", ", listPropertiesWithProblem );

                    var responseErro = new ResponseErrorDto
                    {
                        Message = propertiesWithProblem  != string.Empty ?
                                   $"{apisettings.BadRequestMessage} { propertiesWithProblem  }." : "BadRequest"
                    };

                    var result = new BadRequestObjectResult(responseErro);

                    result.ContentTypes.Add("application/problem+json");
                    result.ContentTypes.Add("application/problem+xml");

                    return result;
                };
            });
        }
    }
}
