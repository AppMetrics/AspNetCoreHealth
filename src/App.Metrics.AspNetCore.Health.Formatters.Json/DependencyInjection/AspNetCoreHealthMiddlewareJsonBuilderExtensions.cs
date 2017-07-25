// <copyright file="AspNetCoreHealthMiddlewareJsonBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Formatters.Json;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Formatters.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AspNetCoreHealthMiddlewareJsonBuilderExtensions
    {
        /// <summary>
        ///     Enables JSON serialization on the health endpoint's response
        /// </summary>
        /// <param name="builder">The health middleware builder.</param>
        /// <returns>The metrics middleware builder checksBuilder</returns>
        public static IAppMetricsHealthMiddlewareBuilder AddJsonFormatters(this IAppMetricsHealthMiddlewareBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddJsonFormatterServices(builder.Services);

            return builder;
        }

        /// <summary>
        ///     Enables JSON serialization on the health endpoint's response
        /// </summary>
        /// <param name="builder">The metrics middleware builder checksBuilder.</param>
        /// <param name="setupAction">The <see cref="JsonSerializerSettings" /> which need to be configured.</param>
        /// <returns>The metrics middleware builder checksBuilder</returns>
        public static IAppMetricsHealthMiddlewareBuilder AddJsonFormatters(
            this IAppMetricsHealthMiddlewareBuilder builder,
            Action<JsonSerializerSettings> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure<AppMetricsHealthJsonOptions>(options => setupAction(options.SerializerSettings));

            return builder;
        }

        /// <summary>
        ///     Enables JSON serialization on the health endpoint's response
        /// </summary>
        /// <param name="builder">The metrics middleware builder checksBuilder.</param>
        /// <param name="setupAction">The <see cref="AppMetricsHealthJsonOptions" /> which need to be configured.</param>
        /// <returns>The metrics middleware builder checksBuilder</returns>
        public static IAppMetricsHealthMiddlewareBuilder AddJsonOptions(
            this IAppMetricsHealthMiddlewareBuilder builder,
            Action<AppMetricsHealthJsonOptions> setupAction)
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

        internal static void AddJsonFormatterServices(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, JsonHealthResponseWriter>());
            // TODO: inject AppMetricsHealthJsonOptions in health core
            services.Replace(
                ServiceDescriptor.Transient<IHealthStatusSerializer>(
                    provider =>
                    {
                        var options = provider.GetRequiredService<IOptions<AppMetricsHealthJsonOptions>>();
                        return new HealthStatusSerializer(options.Value.SerializerSettings);
                    }));
        }
    }
}