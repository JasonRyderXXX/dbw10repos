using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageEXIFScanner
{
    class Program
    {
        static void Main(string[] args) =>
            CreateHostBuilder(args)
            .Build()
            .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile("hostsettings.json", optional: true);
                configHost.AddEnvironmentVariables(prefix: "EXIFHOST_");
                configHost.AddCommandLine(args);
                configHost.AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "ImageExtensions",".jpeg,.jpg,png,.gif" }
                });
            })
            .ConfigureAppConfiguration(appHost =>
            {
                appHost.SetBasePath(Directory.GetCurrentDirectory());
                appHost.AddJsonFile("appsettings.json", optional: true);
                appHost.AddEnvironmentVariables(prefix: "EXIFAPP_");
                appHost.AddCommandLine(args);
                appHost.AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "ImageExtensions",".jpeg,.jpg,png,.gif" }
                });
            })
            .ConfigureServices((hostContext, services) =>
            {
                
                services.AddHostedService<EXIFScanner>();
            });
    }
}
