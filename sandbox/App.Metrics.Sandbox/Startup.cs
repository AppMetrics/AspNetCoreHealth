// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Sandbox
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealth(
                checksRegistry =>
                {
                    checksRegistry.AddProcessPrivateMemorySizeCheck("Private Memory Size", 200);
                    checksRegistry.AddProcessVirtualMemorySizeCheck("Virtual Memory Size", 200);
                    checksRegistry.AddProcessPhysicalMemoryCheck("Working Set", 200);

                    checksRegistry.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                    checksRegistry.AddHttpGetCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));
                });

            services.AddMvc();
        }
    }
}