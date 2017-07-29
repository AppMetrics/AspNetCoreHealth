// <copyright file="MiddlewareHealthChecksAppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health;
using App.Metrics.AspNetCore.Health.Internal;
using App.Metrics.AspNetCore.Health.Options;
using App.Metrics.Health.Builder;
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
        public static IAppMetricsHealthMiddlewareBuilder AddHealthCheckMiddleware(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<AppMetricsHealthMiddlewareOptions>(configuration);

            return services.AddMetricsMiddlewareHealthChecksCore();
        }

        public static IAppMetricsHealthMiddlewareBuilder AddHealthCheckMiddleware(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<AppMetricsHealthMiddlewareOptions> setupOptionsAction)
        {
            services.Configure<AppMetricsHealthMiddlewareOptions>(configuration);
            services.Configure(setupOptionsAction);

            return services.AddMetricsMiddlewareHealthChecksCore();
        }

        public static IAppMetricsHealthMiddlewareBuilder AddHealthCheckMiddleware(
            this IServiceCollection services,
            Action<AppMetricsHealthMiddlewareOptions> setupOptionsAction)
        {
            services.Configure(setupOptionsAction);

            return services.AddMetricsMiddlewareHealthChecksCore();
        }

        public static IAppMetricsHealthMiddlewareBuilder AddHealthCheckMiddleware(this IServiceCollection services)
        {
            return services.AddMetricsMiddlewareHealthChecksCore();
        }

        private static IAppMetricsHealthMiddlewareBuilder AddAppMetricsMiddlewareHealthChecksBuilder(this IServiceCollection services)
        {
            return new AppMetricsHealthMiddlewareBuilder(services);
        }

        private static IAppMetricsHealthMiddlewareBuilder AddMetricsMiddlewareHealthChecksCore(this IServiceCollection services)
        {
            services.TryAddSingleton<IHealthResponseWriter, HealthResponseWriter>();
            services.TryAddSingleton<AppMetricsMiddlewareHealthChecksMarkerService, AppMetricsMiddlewareHealthChecksMarkerService>();
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsHealthMiddlewareOptions>>().Value);
            services.AddSingleton<IStartupFilter>(new HealthCheckStartupFilter());

            return AddAppMetricsMiddlewareHealthChecksBuilder(services);
        }
    }
}