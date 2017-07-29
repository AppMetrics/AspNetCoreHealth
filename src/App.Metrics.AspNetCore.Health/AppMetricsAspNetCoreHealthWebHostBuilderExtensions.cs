// <copyright file="AppMetricsAspNetCoreHealthWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.Health;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsAspNetCoreHealthWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Health Checks Middleware to the <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> request
        ///     execution pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealth(this IWebHostBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ConfigureServices((context, services) =>
            {
                services.Configure<AppMetricsHealthOptions>(context.Configuration.GetSection("AppMetricsHealthOptions"));
                services.AddSingleton<IStartupFilter>(new HealthStartupFilter());
                services.AddHealthMiddlewareCore(context.Configuration.GetSection("AppMetricsAspNetHealthOptions"));
            });

            return builder;
        }
    }
}