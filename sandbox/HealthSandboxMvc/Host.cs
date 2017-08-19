// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Health.Formatters.Ascii;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HealthSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseHealth()
                   // .UseHealth(ConfigureHealthOptions())
                   .UseStartup<Startup>()
                   .Build();

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static Action<HealthWebHostOptions> ConfigureHealthOptions()
        {
            return options =>
            {
                options.EndpointOptions = endpointsOptions =>
                {
                    endpointsOptions.EndpointOutputFormatter = new HealthStatusTextOutputFormatter();
                    endpointsOptions.Endpoint = "/health2";
                };

                options.HealthOptions = metricsOptions => { metricsOptions.Enabled = true; };
            };
        }
    }
}