// <copyright file="HealthApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.AspNetCore.Health.Endpoints.Middleware;
using App.Metrics.Health;
using App.Metrics.Health.Extensions.DependencyInjection.Internal;
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
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics to the request execution pipeline.
    /// </summary>
    public static class HealthApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics health endpoint middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <remarks>
        ///     Uses the matching <see cref="IHealthOutputFormatter" /> given a requests Accept header, otherwise falls back
        ///     to HealthOptions.DefaultMetricsFormatter.
        /// </remarks>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseHealthEndpoint(this IApplicationBuilder app)
        {
            EnsureHealthAdded(app);

            var healthOptions = app.ApplicationServices.GetRequiredService<HealthOptions>();
            var endpointHostingOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthEndpointsHostingOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthEndpointOptions>>();

            UseHealthMiddleware(app, endpointHostingOptionsAccessor, endpointsOptionsAccessor, healthOptions);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics health endpoint middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <param name="formatter">
        ///     Overrides all configured <see cref="IHealthOutputFormatter" />, matching on accept headers
        ///     won't apply.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseHealthEndpoint(this IApplicationBuilder app, IHealthOutputFormatter formatter)
        {
            EnsureHealthAdded(app);

            var healthOptions = app.ApplicationServices.GetRequiredService<HealthOptions>();
            var endpointHostingOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthEndpointsHostingOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthEndpointOptions>>();

            UseHealthMiddleware(app, endpointHostingOptionsAccessor, endpointsOptionsAccessor, healthOptions, formatter);

            return app;
        }

        private static void EnsureHealthAdded(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddHealth was done before calling using middleware.
            // We use the HealthMarkerService to make sure if all the services were added.
            AppMetricsHealthServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);
        }

        private static DefaultHealthResponseWriter GetHealthResponseWriter(
            IServiceProvider serviceProvider,
            IHealthOutputFormatter formatter = null)
        {
            var formatters = serviceProvider.GetRequiredService<IReadOnlyCollection<IHealthOutputFormatter>>();

            if (formatter != null)
            {
                return new DefaultHealthResponseWriter(formatter, formatters);
            }

            var options = serviceProvider.GetRequiredService<IOptions<HealthEndpointOptions>>();
            return new DefaultHealthResponseWriter(options.Value.HealthEndpointOutputFormatter, formatters);
        }

        private static bool ShouldUseHealthEndpoint(
            IOptions<HealthEndpointsHostingOptions> endpointsHostingOptionsAccessor,
            IOptions<HealthEndpointOptions> endpointsOptionsAccessor,
            HealthOptions metricsOptions,
            HttpContext context)
        {
            int? port = null;

            if (endpointsHostingOptionsAccessor.Value.HealthEndpointPort.HasValue)
            {
                port = endpointsHostingOptionsAccessor.Value.HealthEndpointPort.Value;
            }

            return context.Request.Path == endpointsHostingOptionsAccessor.Value.HealthEndpoint &&
                   endpointsOptionsAccessor.Value.HealthEndpointEnabled &&
                   metricsOptions.Enabled &&
                   endpointsHostingOptionsAccessor.Value.HealthEndpoint.IsPresent() &&
                   (!port.HasValue || context.Features.Get<IHttpConnectionFeature>()?.LocalPort == port.Value);
        }

        private static void UseHealthMiddleware(
            IApplicationBuilder app,
            IOptions<HealthEndpointsHostingOptions> endpointsHostingOptionsAccessor,
            IOptions<HealthEndpointOptions> endpointsOptionsAccessor,
            HealthOptions metricsOptions,
            IHealthOutputFormatter formatter = null)
        {
            formatter = formatter ?? endpointsOptionsAccessor.Value.HealthEndpointOutputFormatter;

            app.UseWhen(
                context => ShouldUseHealthEndpoint(endpointsHostingOptionsAccessor, endpointsOptionsAccessor, metricsOptions, context),
                appBuilder =>
                {
                    var responseWriter = GetHealthResponseWriter(app.ApplicationServices, formatter);
                    appBuilder.UseMiddleware<HealthCheckEndpointMiddleware>(responseWriter, endpointsOptionsAccessor.Value.Timeout);
                });
        }
    }
}