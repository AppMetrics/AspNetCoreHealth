// <copyright file="UnhealthyHealthTestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Startup
{
    public class UnhealthyHealthTestStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsMiddlewareHelathCheckOptions = new HealthAspNetCoreOptions
            {
                                                             HealthEndpointEnabled = true
                                                         };

            SetupServices(
                services,
                appMetricsMiddlewareHelathCheckOptions,
                healthChecks: new[] { HealthCheckResult.Healthy(), HealthCheckResult.Degraded(), HealthCheckResult.Unhealthy() });
        }
    }
}