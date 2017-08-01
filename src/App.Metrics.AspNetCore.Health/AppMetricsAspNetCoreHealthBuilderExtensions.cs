// <copyright file="AppMetricsAspNetCoreHealthBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.AspNetCore.Health.Core.Internal;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics AspNet Core health services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class AppMetricsAspNetCoreHealthBuilderExtensions
    {
        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IAppMetricsHealthBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IAppMetricsHealthBuilder" /> to add services to.</param>
        /// <returns>An <see cref="IAppMetricsAspNetCoreHealthBuilder"/> that can be used to further configure the App Metrics AspNet Core health services.</returns>
        public static IAppMetricsAspNetCoreHealthBuilder AddAspNetCoreHealth(this IAppMetricsHealthBuilder builder)
        {
            builder.Services.AddAspNetCoreHealthCore();

            return new AppMetricsAspNetCoreHealthBuilder(builder.Services);
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IAppMetricsHealthBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IAppMetricsHealthBuilder" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="AppMetricsAspNetHealthOptions" />.</param>
        /// <returns>An <see cref="IAppMetricsAspNetCoreHealthBuilder"/> that can be used to further configure the App Metrics AspNet Core health services.</returns>
        public static IAppMetricsAspNetCoreHealthBuilder AddAspNetCoreHealth(
            this IAppMetricsHealthBuilder builder,
            IConfiguration configuration)
        {
            var aspNetCoreBuilder = builder.AddAspNetCoreHealth();

            builder.Services.Configure<AppMetricsAspNetHealthOptions>(configuration);

            return aspNetCoreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IAppMetricsHealthBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IAppMetricsHealthBuilder" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="AppMetricsAspNetHealthOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{AppMetricsAspNetHealthOptions}" /> to configure the provided <see cref="AppMetricsAspNetHealthOptions" />.
        /// </param>
        /// <returns>An <see cref="IAppMetricsAspNetCoreHealthBuilder"/> that can be used to further configure the App Metrics AspNet Core health services.</returns>
        public static IAppMetricsAspNetCoreHealthBuilder AddAspNetCoreHealth(
            this IAppMetricsHealthBuilder builder,
            IConfiguration configuration,
            Action<AppMetricsAspNetHealthOptions> setupAction)
        {
            var aspNetCoreBuilder = builder.AddAspNetCoreHealth();

            builder.Services.Configure<AppMetricsAspNetHealthOptions>(configuration);
            builder.Services.Configure(setupAction);

            return aspNetCoreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IAppMetricsHealthBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IAppMetricsHealthBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{AppMetricsAspNetHealthOptions}" /> to configure the provided <see cref="AppMetricsAspNetHealthOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="AppMetricsAspNetHealthOptions" />.</param>
        /// <returns>An <see cref="IAppMetricsAspNetCoreHealthBuilder"/> that can be used to further configure the App Metrics AspNet Core health services.</returns>
        public static IAppMetricsAspNetCoreHealthBuilder AddAspNetCoreHealth(
            this IAppMetricsHealthBuilder builder,
            Action<AppMetricsAspNetHealthOptions> setupAction,
            IConfiguration configuration)
        {
            var aspNetCoreBuilder = builder.AddAspNetCoreHealth();

            builder.Services.Configure(setupAction);
            builder.Services.Configure<AppMetricsAspNetHealthOptions>(configuration);

            return aspNetCoreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IAppMetricsHealthBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IAppMetricsHealthBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{AppMetricsAspNetHealthOptions}" /> to configure the provided <see cref="AppMetricsAspNetHealthOptions" />.
        /// </param>
        /// <returns>An <see cref="IAppMetricsAspNetCoreHealthBuilder"/> that can be used to further configure the App Metrics AspNet Core health services.</returns>
        public static IAppMetricsAspNetCoreHealthBuilder AddAspNetCoreHealth(
            this IAppMetricsHealthBuilder builder,
            Action<AppMetricsAspNetHealthOptions> setupAction)
        {
            var aspNetCoreBuilder = builder.AddAspNetCoreHealth();

            builder.Services.Configure(setupAction);

            return aspNetCoreBuilder;
        }
    }
}