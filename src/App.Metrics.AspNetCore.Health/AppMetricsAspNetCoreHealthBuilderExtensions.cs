// <copyright file="AppMetricsAspNetCoreHealthBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.Health.Builder;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Health services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class AppMetricsAspNetCoreHealthBuilderExtensions
    {
        public static IAppMetricsAspNetCoreHealthBuilder AddMiddleware(this IAppMetricsHealthBuilder builder)
        {
            builder.Services.AddHealthMiddlewareCore();

            return new AppMetricsAspNetCoreHealthBuilder(builder.Services);
        }

        public static IAppMetricsAspNetCoreHealthBuilder AddMiddleware(
            this IAppMetricsHealthBuilder builder,
            IConfiguration configuration)
        {
            builder.Services.Configure<AppMetricsAspNetHealthOptions>(configuration);

            return builder.AddMiddleware();
        }

        public static IAppMetricsAspNetCoreHealthBuilder AddMiddleware(
            this IAppMetricsHealthBuilder builder,
            IConfiguration configuration,
            Action<AppMetricsAspNetHealthOptions> setupOptionsAction)
        {
            builder.Services.Configure<AppMetricsAspNetHealthOptions>(configuration);
            builder.Services.Configure(setupOptionsAction);

            return builder.AddMiddleware();
        }

        public static IAppMetricsAspNetCoreHealthBuilder AddMiddleware(
            this IAppMetricsHealthBuilder builder,
            Action<AppMetricsAspNetHealthOptions> setupOptionsAction)
        {
            builder.Services.Configure(setupOptionsAction);

            return builder.AddMiddleware();
        }
    }
}