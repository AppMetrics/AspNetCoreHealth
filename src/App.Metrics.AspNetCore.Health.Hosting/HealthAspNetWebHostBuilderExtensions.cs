// <copyright file="HealthAspNetWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;
using App.Metrics.Health.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Health AspNet Core services in an <see cref="IWebHostBuilder" />.
    /// </summary>
    public static class HealthAspNetWebHostBuilderExtensions
    {
        private static bool _healthBuilt;

        public static IWebHostBuilder ConfigureHealthWithDefaults(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, IHealthBuilder> configureHealth)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IMetrics");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var healthBuilder = AppMetricsHealth.CreateDefaultBuilder();
                    configureHealth(context, healthBuilder);
                    healthBuilder.Configuration.ReadFrom(context.Configuration);
                    services.AddHealth(healthBuilder);
                    _healthBuilt = true;
                });
        }

        public static IWebHostBuilder ConfigureHealthWithDefaults(this IWebHostBuilder hostBuilder, Action<IHealthBuilder> configureHealth)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            hostBuilder.ConfigureHealthWithDefaults(
                (context, builder) =>
                {
                    configureHealth(builder);
                });

            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureHealth(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, IHealthBuilder> configureHealth)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddHealth(
                        builder =>
                        {
                            configureHealth(context, builder);
                            builder.Configuration.ReadFrom(context.Configuration);
                            _healthBuilt = true;
                        });
                });
        }

        public static IWebHostBuilder ConfigureHealth(this IWebHostBuilder hostBuilder, Action<IHealthBuilder> configureHealth)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            hostBuilder.ConfigureHealth(
                (context, builder) =>
                {
                    configureHealth(builder);
                });

            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureHealth(this IWebHostBuilder hostBuilder)
        {
            if (_healthBuilt)
            {
                return hostBuilder;
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    if (!_healthBuilt)
                    {
                        var builder = AppMetricsHealth.CreateDefaultBuilder()
                            .Configuration.ReadFrom(context.Configuration);
                        services.AddHealth(builder);
                        _healthBuilt = true;
                    }
                });
        }
    }
}