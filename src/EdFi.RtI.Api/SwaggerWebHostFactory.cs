using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EdFi.RtI.Api
{
    /// <summary>
    /// Class used by Swashbuckle command line tool as an entry point to generate Swagger file in CI/CD
    /// </summary>
    public class SwaggerWebHostFactory
    {
        public static IWebHost CreateWebHost()
        {
            return new WebHostBuilder()
                                    .UseKestrel()
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .ConfigureAppConfiguration(x => x.AddConfiguration(GetConfiguration()))
                                    .UseStartup<SwaggerStartup>()
                                    .Build();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}