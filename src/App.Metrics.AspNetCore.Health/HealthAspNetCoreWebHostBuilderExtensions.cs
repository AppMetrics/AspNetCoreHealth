// <copyright file="HealthAspNetCoreWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.Health;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics AspNet Core Health services in an <see cref="IWebHostBuilder" />.
    /// </summary>
    public static class HealthAspNetCoreWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealth(this IWebHostBuilder hostBuilder)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            hostBuilder.ConfigureServices((context, services) =>
            {
                var endpointsOptions = new HealthEndpointOptions();
                context.Configuration.Bind(nameof(HealthEndpointOptions), endpointsOptions);

                ConfigureServerUrlsKey(hostBuilder, endpointsOptions);
                ConfigureHealthServices(services, context);
            });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthWebHostOptions}" /> to configure the provided <see cref="HealthWebHostOptions" />.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealth(this IWebHostBuilder hostBuilder, Action<HealthWebHostOptions> setupAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            hostBuilder.ConfigureServices((context, services) =>
            {
                var metricsOptions = new HealthWebHostOptions();
                setupAction?.Invoke(metricsOptions);

                var endpointsOptions = new HealthEndpointOptions();
                context.Configuration.Bind(nameof(HealthEndpointOptions), endpointsOptions);
                metricsOptions.EndpointOptions(endpointsOptions);

                ConfigureServerUrlsKey(hostBuilder, endpointsOptions);
                ConfigureHealthServices(services, context, metricsOptions);
            });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{WebHostBuilderContext, HealthWebHostOptions}" /> to configure the provided <see cref="HealthWebHostOptions" />.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealth(this IWebHostBuilder hostBuilder, Action<WebHostBuilderContext, HealthWebHostOptions> setupAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            hostBuilder.ConfigureServices((context, services) =>
            {
                var metricsOptions = new HealthWebHostOptions();
                setupAction?.Invoke(context, metricsOptions);

                var endpointsOptions = new HealthEndpointOptions();
                context.Configuration.Bind(nameof(HealthEndpointOptions), endpointsOptions);
                metricsOptions.EndpointOptions(endpointsOptions);

                ConfigureServerUrlsKey(hostBuilder, endpointsOptions);
                ConfigureHealthServices(services, context, metricsOptions);
            });

            return hostBuilder;
        }

        private static void ConfigureHealthServices(
            IServiceCollection services,
            WebHostBuilderContext context,
            HealthWebHostOptions healthOptions = null)
        {
            if (healthOptions == null)
            {
                healthOptions = new HealthWebHostOptions();
            }

            //
            // Add health services with options, configuration section takes precedence
            //
            var healthBuilder = services.AddHealth(context.Configuration.GetSection(nameof(HealthOptions)), healthOptions.HealthOptions);

            //
            // Add metrics aspnet core health services and options, setup action takes precedence over configuration section
            //
            healthBuilder.AddAspNetCoreHealth(context.Configuration.GetSection(nameof(HealthEndpointOptions)), healthOptions.EndpointOptions);

            //
            // Add the default health startup filter using all health features and endpoint
            //
            services.AddSingleton<IStartupFilter>(new DefaultHealthStartupFilter());
        }

        private static void ConfigureServerUrlsKey(IWebHostBuilder hostBuilder, HealthEndpointOptions endpointsOptions)
        {
            if (endpointsOptions.Port.HasValue)
            {
                var existingUrl = hostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey);
                var additionalUrl = $"http://localhost:{endpointsOptions.Port.Value}";
                hostBuilder.UseSetting(WebHostDefaults.ServerUrlsKey, $"{existingUrl};{additionalUrl}");
            }
        }
    }
}