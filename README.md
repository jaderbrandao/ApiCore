# ApiCore

ApiCore is the most complete library for the API projects. It implements API Versioning and Swagger configuration. It also provides Middlewares but isn't available yet.

# How to use ?

In .NET Core:
```bash
dotnet add package APICore --version 2.1.0
```
If using legacy .NET Framework in Visual Studio
```bash
Install-Package APICore -Version 2.1.0
```
You can also just use the `Manage NuGet Package` window on your project in Visual Studio.

Go on the [NuGet website](https://www.nuget.org/packages/APICore/) for more information.

The packages support:

* With full features: Web API applications, supported by [netcoreapp3.1]

To compile it by yourself, you can git clone, open the project and hit the compile button in Visual Studio.

 # How to get started? 
 First, you need to understand C# language, packages and API Web Application.
 
 Then, after you get the APICore package, edit your Startup.cs:
```csharp
using APICore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Swagger_Versioning
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
            services.AddControllers();

            services.AddApiConfiguration(Configuration);
            services.AddSwaggerConfiguration();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerConfiguration(provider);

        }
    }
}

```

 Add in your appsettings.json, a Title and a Description for your API documentation:
```
 "ApiSettings": {
    "Title": "ApiCore Libray",
    "Description": "Library for APIs projects containing Middlewares and API's documentation through the Swagger framework.",
    "ApiDeprecated": "This Api version is deprecated.",
    "BadRequestMessage": "Exists properties with problems. Try update these problems: "
  }
```

Finally, in your Controller, make a directory like V1;V2... and you can use the same Controller name and different namespaces, and add these atributes :
```csharp
 [ApiController]
 [ApiVersion("1.0", Deprecated =true)]
 [Route("v{version:apiVersion}")]
```
In your actions, you can set the verb and Routes should be the same in V1;V2...
```csharp
 [HttpGet]
 [Route("weatherforecast-list")]
```
Don't forget to apply bin/APICore.xml after checking this option "XML documentation file:" in your Build settings.

For Bad requests, considering DataAnnotations, for example [Required] from body. You can customize messages for this in appsettings.json, see the ApiSettings.cs.
The response ignore null values and use CamelCase.

A new version for Dependency Injection is available, just make a IServiceCollection extension to use this, like example below:
```csharp
 service.AddDependencyInjection(typeof(IService));
```

About the example above, you can put a simple interface to mark your injections for example:
```csharp
 public interface IApiService : IService
```

Please, use github issues for questions or feedback. For confidential requests or specific demands, contact us on [JaderBrandao support](mailto:contato@jaderbrandao.com.br "contato@jaderbrandao.com.br").


## Useful link for a free IDE :
Visual Studio Community Edition : [https://www.visualstudio.com/products/visual-studio-community-vs](https://www.visualstudio.com/products/visual-studio-community-vs "https://www.visualstudio.com/products/visual-studio-community-vs")
 
