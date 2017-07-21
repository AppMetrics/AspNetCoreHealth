// <copyright file="MiddlewareHealthChecksAppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health;
using App.Metrics.AspNetCore.Health.Internal;
using App.Metrics.AspNetCore.Health.Options;
using App.Metrics.Builder;
using App.Metrics.Health;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    public static class MiddlewareHealthChecksAppMetricsBuilderExtensions
    {
        public static IServiceCollection AddHealthCheckMiddleware(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddleware)
        {
            services.Configure<AppMetricsMiddlewareHealthChecksOptions>(configuration);
            return services.AddMetricsMiddlewareHealthChecksCore(setupMiddleware);
        }

        public static IServiceCollection AddHealthCheckMiddleware(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<AppMetricsMiddlewareHealthChecksOptions> setupOptionsAction,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddleware)
        {
            services.Configure<AppMetricsMiddlewareHealthChecksOptions>(configuration);
            services.Configure(setupOptionsAction);

            return services.AddMetricsMiddlewareHealthChecksCore(setupMiddleware);
        }

        public static IServiceCollection AddHealthCheckMiddleware(
            this IServiceCollection services,
            Action<AppMetricsMiddlewareHealthChecksOptions> setupOptionsAction,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddlewareOptionsAction)
        {
            services.Configure(setupOptionsAction);

            return services.AddMetricsMiddlewareHealthChecksCore(setupMiddlewareOptionsAction);
        }

        public static IServiceCollection AddHealthCheckMiddleware(
            this IServiceCollection services,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddleware)
        {
            return services.AddMetricsMiddlewareHealthChecksCore(setupMiddleware);
        }

        private static IAppMetricsMiddlewareHealthChecksOptionsBuilder AddAppMetricsMiddlewareHealthChecksBuilder(this IServiceCollection services)
        {
            return new AppMetricsMiddlewareHealthChecksOptionsBuilder(services);
        }

        private static IServiceCollection AddMetricsMiddlewareHealthChecksCore(
            this IServiceCollection services,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddleware)
        {
            setupMiddleware(services.AddAppMetricsMiddlewareHealthChecksBuilder());

            services.TryAddSingleton(ServiceDescriptor.Transient<IHealthResponseWriter, NoOpHealthStatusResponseWriter>());
            services.TryAddSingleton<AppMetricsMiddlewareHealthChecksMarkerService, AppMetricsMiddlewareHealthChecksMarkerService>();
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsMiddlewareHealthChecksOptions>>().Value);
            services.AddSingleton<IStartupFilter>(new HealthCheckStartupFilter());

            return services;
        }
    }
}