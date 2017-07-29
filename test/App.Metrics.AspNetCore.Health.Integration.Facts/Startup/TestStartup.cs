// <copyright file="TestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health.Options;
using App.Metrics.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Startup
{
    public abstract class TestStartup
    {
        protected void SetupAppBuilder(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) { }

        protected void SetupServices(
            IServiceCollection services,
            AppMetricsHealthMiddlewareOptions appMetricsMiddlewareHealthChecksOptions,
            IEnumerable<HealthCheckResult> healthChecks = null)
        {
            services.AddLogging().AddRouting(options => { options.LowercaseUrls = true; });

            var startupAssemblyName = typeof(TestStartup).Assembly.GetName().Name;

#pragma warning disable CS0612
            services.
                AddHealth(
                    startupAssemblyName,
                    checksRegistry =>
                    {
                        var checks = healthChecks?.ToList() ?? new List<HealthCheckResult>();

                        for (var i = 0; i < checks.Count; i++)
                        {
                            var check = checks[i];
                            checksRegistry.AddCheck("Check" + i, () => new ValueTask<HealthCheckResult>(check));
                        }
                    });
#pragma warning restore CS0612

            services.AddHealthCheckMiddleware(
                options =>
                {
                    options.HealthEndpointEnabled = appMetricsMiddlewareHealthChecksOptions.HealthEndpointEnabled;
                    options.HealthEndpoint = appMetricsMiddlewareHealthChecksOptions.HealthEndpoint;
                });
        }
    }
}