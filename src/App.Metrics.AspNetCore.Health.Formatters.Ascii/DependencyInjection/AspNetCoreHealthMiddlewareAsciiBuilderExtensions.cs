// <copyright file="AspNetCoreHealthMiddlewareAsciiBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Formatters.Ascii;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AspNetCoreHealthMiddlewareAsciiBuilderExtensions
    {
        /// <summary>
        ///     Enables Plain Text serialization on the health endpoint's response
        /// </summary>
        /// <param name="builder">The health middleware builder.</param>
        /// <returns>The metrics middleware builder checksBuilder</returns>
        public static IAppMetricsHealthMiddlewareBuilder AddAsciiFormatters(this IAppMetricsHealthMiddlewareBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddAsciiFormatterServices(builder.Services);

            return builder;
        }

        /// <summary>
        ///     Enables Plain Text serialization on the health endpoint's response
        /// </summary>
        /// <param name="builder">The metrics middleware builder checksBuilder.</param>
        /// <param name="setupAction">The <see cref="AppMetricsHealthAsciiOptions"/> which need to be configured.</param>
        /// <returns>The metrics middleware builder checksBuilder</returns>
        public static IAppMetricsHealthMiddlewareBuilder AddAsciiOptions(
            this IAppMetricsHealthMiddlewareBuilder builder,
            Action<AppMetricsHealthAsciiOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure(setupAction);

            return builder;
        }

        internal static void AddAsciiFormatterServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<AppMetricsHealthOptions>, AppMetricsHealthAsciiOptionsSetup>());

            // TODO: inject AppMetricsHealthAsciiOptions in health core
            services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, AsciiHealthResponseWriter>());
        }
    }
}