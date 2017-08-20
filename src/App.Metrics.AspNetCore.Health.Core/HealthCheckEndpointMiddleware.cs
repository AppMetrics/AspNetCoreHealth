// <copyright file="HealthCheckEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace App.Metrics.AspNetCore.Health.Core
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class HealthCheckEndpointMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly IProvideHealth _health;
        private readonly IHealthResponseWriter _healthResponseWriter;
        private readonly ILogger<HealthCheckEndpointMiddleware> _logger;
        private readonly TimeSpan _timeout;

        public HealthCheckEndpointMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IProvideHealth health,
            IHealthResponseWriter healthResponseWriter,
            TimeSpan timeout)
        {
            _next = next;
            _health = health;
            _logger = loggerFactory.CreateLogger<HealthCheckEndpointMiddleware>();
            _healthResponseWriter = healthResponseWriter ?? throw new ArgumentNullException(nameof(healthResponseWriter));
            _timeout = timeout;
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting(GetType());

            var healthCheckCancellationTokenSource = new CancellationTokenSource(_timeout);

            using (var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(healthCheckCancellationTokenSource.Token, context.RequestAborted))
            {
                try
                {
                    var healthStatus = await _health.ReadAsync(cancellationTokenSource.Token);

                    await _healthResponseWriter.WriteAsync(context, healthStatus, cancellationTokenSource.Token);
                }
                catch (OperationCanceledException e)
                {
                    var responseFeature = context.Response.HttpContext.Features.Get<IHttpResponseFeature>();

                    if (healthCheckCancellationTokenSource.IsCancellationRequested)
                    {
                        responseFeature.ReasonPhrase = "Health Check Middleware timed out.";
                        _logger.MiddlewareFailed(GetType(), e, responseFeature.ReasonPhrase);
                    }
                    else if (context.RequestAborted.IsCancellationRequested)
                    {
                        responseFeature.ReasonPhrase = "Health Check Middleware request aborted.";
                        _logger.MiddlewareFailed(GetType(), e, responseFeature.ReasonPhrase);
                    }

                    context.SetNoCacheHeaders();
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    context.Response.Headers[HeaderNames.ContentType] = new[] { context.Request.ContentType };
                }
            }

            _logger.MiddlewareExecuted(GetType());
        }
    }
}