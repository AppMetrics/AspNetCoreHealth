// <copyright file="HealthAspNetCoreHealthCoreServiceCollectionExtensions.cs" company="Allan Hardy">
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
    public static class HealthAspNetCoreHealthCoreServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IHealthAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core health
        ///     services.
        /// </returns>
        public static IHealthAspNetCoreCoreBuilder AddAspNetCoreHealthCore(this IServiceCollection services)
        {
            ConfigureDefaultServices(services);
            AddAppMetricsAspNetCoreHealthServices(services);

            return new HealthAspNetCoreCoreBuilder(services);
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="HealthAspNetCoreOptions" />.</param>
        /// <returns>
        ///     An <see cref="IHealthAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core health
        ///     services.
        /// </returns>
        public static IHealthAspNetCoreCoreBuilder AddAspNetCoreHealthCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var coreBuilder = services.AddAspNetCoreHealthCore();

            services.Configure<HealthAspNetCoreOptions>(configuration);

            return coreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="HealthAspNetCoreOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthAspNetCoreOptions}" /> to configure the provided <see cref="HealthAspNetCoreOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core health
        ///     services.
        /// </returns>
        public static IHealthAspNetCoreCoreBuilder AddAspNetCoreHealthCore(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<HealthAspNetCoreOptions> setupAction)
        {
            var coreBuilder = services.AddAspNetCoreHealthCore();

            services.Configure<HealthAspNetCoreOptions>(configuration);
            services.Configure(setupAction);

            return coreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthAspNetCoreOptions}" /> to configure the provided <see cref="HealthAspNetCoreOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="HealthAspNetCoreOptions" />.</param>
        /// <returns>
        ///     An <see cref="IHealthAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core health
        ///     services.
        /// </returns>
        public static IHealthAspNetCoreCoreBuilder AddAspNetCoreHealthCore(
            this IServiceCollection services,
            Action<HealthAspNetCoreOptions> setupAction,
            IConfiguration configuration)
        {
            var coreBuilder = services.AddAspNetCoreHealthCore();

            services.Configure(setupAction);
            services.Configure<HealthAspNetCoreOptions>(configuration);

            return coreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthAspNetCoreOptions}" /> to configure the provided <see cref="HealthAspNetCoreOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core health
        ///     services.
        /// </returns>
        public static IHealthAspNetCoreCoreBuilder AddAspNetCoreHealthCore(
            this IServiceCollection services,
            Action<HealthAspNetCoreOptions> setupAction)
        {
            var coreBuilder = services.AddAspNetCoreHealthCore();

            services.Configure(setupAction);

            return coreBuilder;
        }

        internal static void AddAppMetricsAspNetCoreHealthServices(IServiceCollection services)
        {
            services.TryAddSingleton<HealthAspNetCoreMarkerService, HealthAspNetCoreMarkerService>();
            services.TryAddSingleton<IHealthResponseWriter, HealthResponseWriter>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<HealthAspNetCoreOptions>, HealthAspNetCoreHealthAspNetCoreOptionsSetup>());
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<HealthAspNetCoreOptions>>().Value);
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddRouting();
        }
    }
}