// <copyright file="HealthCheckStartupFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Options;
using App.Metrics.Health.DependencyInjection.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Health
{
    public class HealthCheckStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                // Verify if AddHealth was done before calling UseHealth
                // We use the HealthCheckMarkerService to make sure if all the services were added.
                AppMetricsHealthServicesHelper.ThrowIfHealthChecksNotRegistered(app.ApplicationServices);

                var aspNetMetricsMiddlewareHealthChecksOptions = app.ApplicationServices.GetRequiredService<AppMetricsMiddlewareHealthChecksOptions>();

                if (aspNetMetricsMiddlewareHealthChecksOptions.HealthEndpointEnabled)
                {
                    app.UseMiddleware<HealthCheckEndpointMiddleware>();
                }

                next(app);
            };
        }
    }
}
