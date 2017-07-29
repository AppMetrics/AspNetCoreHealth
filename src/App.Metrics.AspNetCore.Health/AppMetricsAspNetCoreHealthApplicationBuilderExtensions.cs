// <copyright file="AppMetricsAspNetCoreHealthApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.Health.DependencyInjection.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Health
{
    public static class AppMetricsAspNetCoreHealthApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds App Metrics Health to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseHealth(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // TODO: Don't want this copy and pasted from HealthStartupFilter

            // Verify if AddHealth was done before calling UseHealth
            // We use the HealthCheckMarkerService to make sure if all the services were added.
            AppMetricsHealthServicesHelper.ThrowIfHealthChecksNotRegistered(app.ApplicationServices);

            var aspNetMetricsMiddlewareHealthChecksOptions = app.ApplicationServices.GetRequiredService<AppMetricsAspNetHealthOptions>();

            if (aspNetMetricsMiddlewareHealthChecksOptions.HealthEndpointEnabled)
            {
                app.UseMiddleware<HealthCheckEndpointMiddleware>();
            }

            return app;
        }
    }
}
