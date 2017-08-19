// <copyright file="DefaultHealthResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace App.Metrics.AspNetCore.Health.Core
{
    public class DefaultHealthResponseWriter : IHealthResponseWriter
    {
        private readonly IHealthOutputFormatter _formatter;
        private readonly HealthOptions _healthOptions;

        public DefaultHealthResponseWriter(
            IOptions<HealthOptions> healthOptionsAccessor,
            IOptions<HealthEndpointOptions> healthMiddlewareOptionsAccessor,
            IHealthOutputFormatter formatter)
        {
            if (healthMiddlewareOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(healthMiddlewareOptionsAccessor));
            }

            _formatter = formatter;
            _healthOptions = healthOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(healthOptionsAccessor));
        }

        public DefaultHealthResponseWriter(
            IOptions<HealthOptions> healthOptionsAccessor,
            IOptions<HealthEndpointOptions> healthMiddlewareOptionsAccessor)
        {
            if (healthMiddlewareOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(healthMiddlewareOptionsAccessor));
            }

            _healthOptions = healthOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(healthOptionsAccessor));
        }

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default(CancellationToken))
        {
            var formatter = _formatter ?? context.Request.GetTypedHeaders().ResolveFormatter(
                                _healthOptions.DefaultOutputFormatter,
                                metricsMediaTypeValue => _healthOptions.OutputFormatters.GetType(metricsMediaTypeValue));

            context.SetNoCacheHeaders();

            if (formatter == default(IHealthOutputFormatter))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                context.Response.Headers[HeaderNames.ContentType] = new[] { context.Request.ContentType };
                return Task.CompletedTask;
            }

            SetResponseStatusCode(context, healthStatus);

            context.Response.Headers[HeaderNames.ContentType] = new[] { formatter.MediaType.ContentType };

            return formatter.WriteAsync(context.Response.Body, healthStatus, token);
        }

        private static void SetResponseStatusCode(HttpContext context, HealthStatus healthStatus)
        {
            var responseStatusCode = HttpStatusCode.OK;

            if (healthStatus.Status.IsUnhealthy())
            {
                responseStatusCode = HttpStatusCode.ServiceUnavailable;
            }

            if (healthStatus.Status.IsDegraded())
            {
                responseStatusCode = HttpStatusCode.OK;
                context.Response.Headers[HeaderNames.Warning] = new[] { "Warning: 100 'Degraded'" };
            }

            context.Response.StatusCode = (int)responseStatusCode;
        }
    }
}