﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
namespace BackgroundHostedServices
{
    public class Startup
    {
        public Startup(IHostingEnvironment env = null)
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHostedService, MessagesServiceHost>();

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();


            app.Run(async (context) =>
            {
                context.Response.ContentType = "application/json";
                await context.Response
                    .WriteAsync(" Hosted by Kestrel");

                if (serverAddressesFeature != null)
                {
                    await context.Response
                        .WriteAsync("\n Listening on the following addresses: " +
                            string.Join(", ", serverAddressesFeature.Addresses) +
                            "");
                }

                await context.Response.WriteAsync($"\n Request URL: {context.Request.GetDisplayUrl()}");
            });
        }

    }
}