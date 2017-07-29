﻿// <copyright file="DefaultTestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Health.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Startup
{
    public class DefaultTestStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsMiddlewareHealthChecksOptions = new AppMetricsAspNetHealthOptions
            {
                                                  HealthEndpointEnabled = true,
                                              };

            SetupServices(services, appMetricsMiddlewareHealthChecksOptions);
        }
    }
}