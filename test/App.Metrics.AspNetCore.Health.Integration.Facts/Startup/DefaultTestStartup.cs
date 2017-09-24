// <copyright file="DefaultTestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Health.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Startup
{
    // ReSharper disable UnusedMember.Global
    public class DefaultTestStartup : TestStartup
        // ReSharper restore UnusedMember.Global
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHealthEndpoint();

            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsMiddlewareHealthChecksOptions = new HealthEndpointOptions { HealthEndpointEnabled = true };

            SetupServices(services, appMetricsMiddlewareHealthChecksOptions);
        }
    }
}