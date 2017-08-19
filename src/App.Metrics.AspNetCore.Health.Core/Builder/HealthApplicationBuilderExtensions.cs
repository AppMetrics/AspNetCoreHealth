// <copyright file="HealthApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.Health;
using App.Metrics.Health.DependencyInjection.Internal;
using App.Metrics.Health.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
            EnsureHealthAdded(app);

            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthEndpointOptions>>();
            var healthOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthOptions>>();

            UseHealthMiddleware(app, endpointsOptionsAccessor, healthOptionsAccessor);

            return app;
        }

        /// <summary>
        /// Adds App Metrics Health to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="formatter">
        ///     Overrides the default use of <see cref="IHealthOutputFormatter" /> with the
        ///     <see cref="IHealthOutputFormatter" /> specified.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseHealth(this IApplicationBuilder app, IHealthOutputFormatter formatter)
        {
            EnsureHealthAdded(app);

            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthEndpointOptions>>();
            var healthOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthOptions>>();

            UseHealthMiddleware(app, endpointsOptionsAccessor, healthOptionsAccessor, formatter);

            return app;
        }

        private static void UseHealthMiddleware(
            IApplicationBuilder app,
            IOptions<HealthEndpointOptions> endpointsOptionsAccessor,
            IOptions<HealthOptions> healthOptionsAccessor,
            IHealthOutputFormatter formatter = null)
        {
            formatter = formatter ?? endpointsOptionsAccessor.Value.EndpointOutputFormatter;

            app.UseWhen(
                context => ShouldUseHealthEndpoint(endpointsOptionsAccessor, healthOptionsAccessor, context),
                appBuilder =>
                {
                    var responseWriter = GetHealthResponseWriter(appBuilder.ApplicationServices, formatter);
                    appBuilder.UseMiddleware<HealthCheckEndpointMiddleware>(responseWriter);
                });
        }

        private static bool ShouldUseHealthEndpoint(
            IOptions<HealthEndpointOptions> endpointsOptionsAccessor,
            IOptions<HealthOptions> healthOptionsAccessor,
            HttpContext context)
        {
            return context.Request.Path == endpointsOptionsAccessor.Value.Endpoint &&
                   healthOptionsAccessor.Value.Enabled &&
                   endpointsOptionsAccessor.Value.Enabled &&
                   endpointsOptionsAccessor.Value.Endpoint.IsPresent() &&
                   (!endpointsOptionsAccessor.Value.Port.HasValue || context.Features.Get<IHttpConnectionFeature>()?.LocalPort == endpointsOptionsAccessor.Value.Port.Value);
        }

        private static DefaultHealthResponseWriter GetHealthResponseWriter(IServiceProvider serviceProvider, IHealthOutputFormatter formatter = null)
        {
            var options = serviceProvider.GetRequiredService<IOptions<HealthOptions>>();
            var endpointsOptionsAccessor = serviceProvider.GetRequiredService<IOptions<HealthEndpointOptions>>();

            return formatter == null
                ? new DefaultHealthResponseWriter(options, endpointsOptionsAccessor)
                : new DefaultHealthResponseWriter(options, endpointsOptionsAccessor, formatter);
        }

        private static void EnsureHealthAdded(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddHealth was done before calling UseHealth
            // We use the HealthCheckMarkerService to make sure if all the services were added.
            AppMetricsHealthServicesHelper.ThrowIfHealthChecksNotRegistered(app.ApplicationServices);
        }
    }
}
