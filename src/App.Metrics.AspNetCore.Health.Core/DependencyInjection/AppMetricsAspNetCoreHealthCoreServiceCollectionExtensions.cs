// <copyright file="AppMetricsAspNetCoreHealthCoreServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.AspNetCore.Health.Core.DependencyInjection.Internal;
using App.Metrics.AspNetCore.Health.Core.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsAspNetCoreHealthCoreServiceCollectionExtensions
    {
        public static IAppMetricsAspNetCoreHealthCoreBuilder AddHealthMiddlewareCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<AppMetricsAspNetHealthOptions>(configuration);

            return services.AddAppMetricsAspNetCoreHealthServices();
        }

        public static IAppMetricsAspNetCoreHealthCoreBuilder AddHealthMiddlewareCore(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<AppMetricsAspNetHealthOptions> setupOptionsAction)
        {
            services.Configure<AppMetricsAspNetHealthOptions>(configuration);
            services.Configure(setupOptionsAction);

            return services.AddAppMetricsAspNetCoreHealthServices();
        }

        public static IAppMetricsAspNetCoreHealthCoreBuilder AddHealthMiddlewareCore(
            this IServiceCollection services,
            Action<AppMetricsAspNetHealthOptions> setupOptionsAction)
        {
            services.Configure(setupOptionsAction);

            return services.AddAppMetricsAspNetCoreHealthServices();
        }

        public static IAppMetricsAspNetCoreHealthCoreBuilder AddHealthMiddlewareCore(this IServiceCollection services)
        {
            return services.AddAppMetricsAspNetCoreHealthServices();
        }

        private static IAppMetricsAspNetCoreHealthCoreBuilder AddAppMetricsAspNetCoreHealthServices(this IServiceCollection services)
        {
            services.TryAddSingleton<AppMetricsAspNetCoreHealthMarkerService, AppMetricsAspNetCoreHealthMarkerService>();
            services.TryAddSingleton<IHealthResponseWriter, HealthResponseWriter>();
            services.TryAddSingleton<IConfigureOptions<AppMetricsAspNetHealthOptions>, AppMetricsAspNetCoreHealthOptionsSetup>();
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsAspNetHealthOptions>>().Value);

            return new AppMetricsAspNetCoreHealthCoreBuilder(services);
        }
    }
}