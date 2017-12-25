using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace BackgroundHostedServices
{
    /// <summary>
    /// Executing the "dotnet run command in the application folder will run this app.
    /// </summary>
    public class Program
    {
        public static string Server;


        public static void Main(string[] args)
        {
            Console.WriteLine("Running demo with Kestrel.");

            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var builder = new WebHostBuilder()
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                   // TODO: support end-point for self checking, monitoring, administration, service / task cancellation.... 
                   options.Listen(IPAddress.Any, 32754); // docker outer port
                });

            var host = builder.Build();
            host.Run();
        }

    }
}