// <copyright file="HealthAspNetCoreWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extension methods for <see cref="IWebHostBuilder"/> to add App Metrics health to the request execution pipeline.
    /// </summary>
    public static class HealthAspNetCoreWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Health Checks Middleware to the <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> request
        ///     execution pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="IWebHostBuilder" />.</param>
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
                services.AddHealth(context.Configuration.GetSection("HealthOptions"));
                services.AddAspNetCoreHealthCore(context.Configuration.GetSection("HealthAspNetCoreOptions"));
                services.AddSingleton<IStartupFilter>(new HealthStartupFilter());
            });

            return builder;
        }
    }
}