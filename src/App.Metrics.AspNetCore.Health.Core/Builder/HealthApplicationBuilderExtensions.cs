// <copyright file="HealthApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.Health.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/> to add App Metrics health to the request execution pipeline.
    /// </summary>
    public static class HealthApplicationBuilderExtensions
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

            // Verify if AddHealth was done before calling UseHealth
            // We use the HealthCheckMarkerService to make sure if all the services were added.
            AppMetricsHealthServicesHelper.ThrowIfHealthChecksNotRegistered(app.ApplicationServices);

            var optionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthAspNetCoreOptions>>();

            if (optionsAccessor.Value.HealthEndpointEnabled)
            {
                app.UseMiddleware<HealthCheckEndpointMiddleware>();
            }

            return app;
        }
    }
}
