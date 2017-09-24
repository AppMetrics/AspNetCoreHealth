// <copyright file="HealthAspNetEndpointWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.AspNetCore.Health.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    public static class HealthAspNetEndpointWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Helath services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealthEndpoints(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddHealthEndpoints(context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultHealthEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="HealthEndpointOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealthEndpoints(
            this IWebHostBuilder hostBuilder,
            Action<HealthEndpointOptions> optionsDelegate)
        {
            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddHealthEndpoints(optionsDelegate, context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultHealthEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupDelegate">A callback to configure <see cref="HealthEndpointOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealthEndpoints(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, HealthEndpointOptions> setupDelegate)
        {
            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var endpointOptions = new HealthEndpointOptions();
                    services.AddHealthEndpoints(
                        options => setupDelegate(context, endpointOptions),
                        context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultHealthEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> containing <see cref="HealthEndpointOptions" /></param>
        /// <param name="optionsDelegate">A callback to configure <see cref="HealthEndpointOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealthEndpoints(
            this IWebHostBuilder hostBuilder,
            IConfiguration configuration,
            Action<HealthEndpointOptions> optionsDelegate)
        {
            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                services =>
                {
                    services.AddHealthEndpoints(optionsDelegate, configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultHealthEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureAppHealthHostingConfiguration(
            this IWebHostBuilder hostBuilder,
            Action<HealthEndpointsHostingOptions> setupHostingConfiguration)
        {
            var metricsEndpointHostingOptions = new HealthEndpointsHostingOptions();
            setupHostingConfiguration(metricsEndpointHostingOptions);

            var ports = new List<int>();

            if (metricsEndpointHostingOptions.HealthEndpointPort.HasValue)
            {
                Console.WriteLine($"Hosting {metricsEndpointHostingOptions.HealthEndpoint} on port {metricsEndpointHostingOptions.HealthEndpointPort.Value}");
                ports.Add(metricsEndpointHostingOptions.HealthEndpointPort.Value);
            }

            if (ports.Any())
            {
                var existingUrl = hostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey);
                var additionalUrls = string.Join(";", ports.Distinct().Select(p => $"http://localhost:{p}/"));
                hostBuilder.UseSetting(WebHostDefaults.ServerUrlsKey, $"{existingUrl};{additionalUrls}");
            }

            hostBuilder.ConfigureServices(services => services.Configure(setupHostingConfiguration));

            return hostBuilder;
        }
    }
}