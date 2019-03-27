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

namespace App.Metrics.AspNetCore.Health.Endpoints.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class HealthCheckEndpointMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly IRunHealthChecks _healthCheckRunner;
        private readonly IHealthResponseWriter _healthResponseWriter;
        private readonly IHealthAuthorizationFilter _healthAuthorizationFilter;
        private readonly ILogger<HealthCheckEndpointMiddleware> _logger;
        private readonly TimeSpan _timeout;

        // ReSharper disable UnusedParameter.Local
        public HealthCheckEndpointMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IRunHealthChecks healthCheckRunner,
            IHealthResponseWriter healthResponseWriter,
            IHealthAuthorizationFilter healthAuthorizationFilter,
            TimeSpan timeout)
            // ReSharper restore UnusedParameter.Local
        {
            _healthCheckRunner = healthCheckRunner;
            _logger = loggerFactory.CreateLogger<HealthCheckEndpointMiddleware>();
            _healthResponseWriter = healthResponseWriter ?? throw new ArgumentNullException(nameof(healthResponseWriter));
            _healthAuthorizationFilter = healthAuthorizationFilter ?? new AuthorizedHealthAuthorizationFilter();
            _timeout = timeout <= TimeSpan.Zero ? TimeSpan.FromSeconds(20) : timeout;
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<HealthCheckEndpointMiddleware>();

            var healthCheckCancellationTokenSource = new CancellationTokenSource(_timeout);

            using (var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(healthCheckCancellationTokenSource.Token, context.RequestAborted))
            {
                try
                {
                  if (!_healthAuthorizationFilter.Authorized(context))
                  {
                    _logger.MiddlewareAuthorizationInvalid<HealthCheckEndpointMiddleware>();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid authorization.", cancellationToken: cancellationTokenSource.Token);
                  }
                  else
                  {
                    var healthStatus = await _healthCheckRunner.ReadAsync(cancellationTokenSource.Token);

                    await _healthResponseWriter.WriteAsync(context, healthStatus, cancellationTokenSource.Token);
                  }
                }
                catch (OperationCanceledException e)
                {
                    var responseFeature = context.Response.HttpContext.Features.Get<IHttpResponseFeature>();

                    if (healthCheckCancellationTokenSource.IsCancellationRequested)
                    {
                        responseFeature.ReasonPhrase = "Health Check Middleware timed out.";
                        _logger.MiddlewareFailed<HealthCheckEndpointMiddleware>(e, responseFeature.ReasonPhrase);
                    }
                    else if (context.RequestAborted.IsCancellationRequested)
                    {
                        responseFeature.ReasonPhrase = "Health Check Middleware request aborted.";
                        _logger.MiddlewareFailed<HealthCheckEndpointMiddleware>(e, responseFeature.ReasonPhrase);
                    }

                    context.SetNoCacheHeaders();
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    context.Response.Headers[HeaderNames.ContentType] = new[] { context.Request.ContentType };
                }
            }

            _logger.MiddlewareExecuted<HealthCheckEndpointMiddleware>();
        }
    }
}