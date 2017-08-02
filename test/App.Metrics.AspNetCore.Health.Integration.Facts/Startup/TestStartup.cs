// <copyright file="TestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Startup
{
    public abstract class TestStartup
    {
        protected void SetupAppBuilder(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) { app.UseHealth(); }

        protected void SetupServices(
            IServiceCollection services,
            HealthAspNetCoreOptions healthMiddlewareCoreChecksOptions,
            IEnumerable<HealthCheckResult> healthChecks = null)
        {
            services.AddOptions();
            services.AddLogging().AddRouting(options => { options.LowercaseUrls = true; });

            var startupAssemblyName = typeof(TestStartup).Assembly.GetName().Name;

#pragma warning disable CS0612
            var healthBuilder = services.
                AddHealth(
                    startupAssemblyName,
                    options =>
                    {
                        var checks = healthChecks?.ToList() ?? new List<HealthCheckResult>();

                        for (var i = 0; i < checks.Count; i++)
                        {
                            var check = checks[i];
                            options.Checks.AddCheck("Check" + i, () => new ValueTask<HealthCheckResult>(check));
                        }
                    });
#pragma warning restore CS0612

            healthBuilder.AddAspNetCoreHealth(
                options =>
                {
                    options.HealthEndpointEnabled = healthMiddlewareCoreChecksOptions.HealthEndpointEnabled;
                    options.HealthEndpoint = healthMiddlewareCoreChecksOptions.HealthEndpoint;
                });
        }
    }
}