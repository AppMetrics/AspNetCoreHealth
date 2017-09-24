// <copyright file="HealthAspNetCoreHealthEndpointsServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.AspNetCore.Health;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.AspNetCore.Health.Core.Internal.NoOp;
using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.AspNetCore.Health.Endpoints.Internal;
using App.Metrics.Health;
using App.Metrics.Health.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class HealthAspNetCoreHealthEndpointsServiceCollectionExtensions
    {
        private static readonly string DefaultConfigSection = nameof(HealthEndpointOptions);

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(this IServiceCollection services)
        {
            AddHealthEndpointsServices(services);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="HealthEndpointOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHealthEndpoints(configuration.GetSection(DefaultConfigSection));

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="HealthEndpointOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            IConfigurationSection configuration)
        {
            services.AddHealthEndpoints();

            services.Configure<HealthEndpointOptions>(configuration);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="HealthEndpointOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointOptions}" /> to configure the provided <see cref="HealthEndpointOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<HealthEndpointOptions> setupAction)
        {
            services.AddHealthEndpoints(configuration.GetSection(DefaultConfigSection), setupAction);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="HealthEndpointOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointOptions}" /> to configure the provided <see cref="HealthEndpointOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            IConfigurationSection configuration,
            Action<HealthEndpointOptions> setupAction)
        {
            services.AddHealthEndpoints();

            services.Configure<HealthEndpointOptions>(configuration);
            services.Configure(setupAction);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointOptions}" /> to configure the provided <see cref="HealthEndpointOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="HealthEndpointOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            Action<HealthEndpointOptions> setupAction,
            IConfiguration configuration)
        {
            services.AddHealthEndpoints(setupAction, configuration.GetSection(DefaultConfigSection));

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointOptions}" /> to configure the provided <see cref="HealthEndpointOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="HealthEndpointOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            Action<HealthEndpointOptions> setupAction,
            IConfigurationSection configuration)
        {
            services.AddHealthEndpoints();

            services.Configure(setupAction);
            services.Configure<HealthEndpointOptions>(configuration);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointOptions}" /> to configure the provided <see cref="HealthEndpointOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            Action<HealthEndpointOptions> setupAction)
        {
            services.AddHealthEndpoints();

            services.Configure(setupAction);

            return services;
        }

        internal static void AddHealthEndpointsServices(IServiceCollection services)
        {
            var endpointOptionsDescriptor = ServiceDescriptor.Singleton<IConfigureOptions<HealthEndpointOptions>, HealthEndpointsOptionsSetup>();
            services.TryAddEnumerable(endpointOptionsDescriptor);

            services.TryAddSingleton<IHealthResponseWriter>(provider => ResolveHealthResponseWriter(provider));
        }

        internal static IHealthResponseWriter ResolveHealthResponseWriter(IServiceProvider provider, IHealthOutputFormatter formatter = null)
        {
            var endpointOptions = provider.GetRequiredService<IOptions<HealthEndpointOptions>>();
            var health = provider.GetRequiredService<IHealthRoot>();

            if (health.Options.Enabled && endpointOptions.Value.HealthEndpointEnabled && health.OutputHealthFormatters.Any())
            {
                if (formatter == null)
                {
                    var fallbackFormatter = endpointOptions.Value.HealthEndpointOutputFormatter ?? health.DefaultOutputHealthFormatter;

                    return new DefaultHealthResponseWriter(fallbackFormatter, health.OutputHealthFormatters);
                }

                return new DefaultHealthResponseWriter(formatter, health.OutputHealthFormatters);
            }

            return new NoOpHealthStatusResponseWriter();
        }
    }
}
