using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Settings;
using CPER2G3.Earth4Sport.AzureFunction.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;

[assembly: WebJobsStartup(typeof(CPER2G3.Earth4Sport.SwashbuckleStartup))]
namespace CPER2G3.Earth4Sport
{
    public class SwashbuckleStartup : FunctionsStartup {
        public override void Configure(IFunctionsHostBuilder builder) {
           
            builder.Services.AddSingleton<IDAL, DAL_mongo>();
            builder.Services.AddSingleton<IUserService, UserService>(); 
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), opts => {
                opts.AddCodeParameter = true;
                opts.PrependOperationWithRoutePrefix = true;
                opts.Documents = new[] {
                    new SwaggerDocument {
                        Name = "v1",
                        Title = "Swagger document",
                        Description = "Integrate Swagger UI With Azure Functions",
                        Version = "v2"
                    }
                };
                opts.ConfigureSwaggerGen = x => {
                    x.CustomOperationIds(apiDesc => {
                        return apiDesc.TryGetMethodInfo(out MethodInfo mInfo) ? mInfo.Name : default(Guid).ToString();
                    });
                };
            });
        }
    }
}