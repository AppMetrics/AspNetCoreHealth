// <copyright file="HealthChecksAppMetricsWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    public static class HealthChecksAppMetricsWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Health Checks Middleware to the <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> request
        ///     execution pipeline.
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
                services.Configure<AppMetricsHealthOptions>(context.Configuration.GetSection("AppMetricsHealthOptions"));

                services.AddHealthCheckMiddleware(
                        configuration: context.Configuration.GetSection("AppMetricsMiddlewareHealthChecksOptions"),
                        setupMiddleware: optionsBuilder => optionsBuilder.AddAsciiFormatters());
            });

            return hostBuilder;
        }
    }
}