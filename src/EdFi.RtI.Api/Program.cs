using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EdFi.RtI.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IncreaseThreadsForRedis();
            var configuration = GetConfiguration();
            CreateHostBuilder(configuration, args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration(x => x.AddConfiguration(configuration));
                webBuilder.UseStartup<Startup>();
            });

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static void IncreaseThreadsForRedis()
        {
            ThreadPool.SetMinThreads(10, 10);
        }
    }
}
