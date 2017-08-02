// <copyright file="HealthCheckEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Core
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class HealthCheckEndpointMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly HealthAspNetCoreOptions _appMiddlewareOptions;
        private readonly IProvideHealth _health;
        private readonly IHealthResponseWriter _healthResponseWriter;
        private readonly ILogger<HealthCheckEndpointMiddleware> _logger;

        public HealthCheckEndpointMiddleware(
            RequestDelegate next,
            HealthAspNetCoreOptions appMiddlewareOptions,
            ILoggerFactory loggerFactory,
            IProvideHealth health,
            IHealthResponseWriter healthResponseWriter)
        {
            _next = next;
            _appMiddlewareOptions = appMiddlewareOptions;
            _health = health;
            _logger = loggerFactory.CreateLogger<HealthCheckEndpointMiddleware>();
            _healthResponseWriter = healthResponseWriter ?? throw new ArgumentNullException(nameof(healthResponseWriter));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (_appMiddlewareOptions.HealthEndpointEnabled &&
                _appMiddlewareOptions.HealthEndpoint.IsPresent() &&
                _appMiddlewareOptions.HealthEndpoint == context.Request.Path)
            {
                _logger.MiddlewareExecuting(GetType());

                var healthStatus = await _health.ReadAsync(context.RequestAborted);

                await _healthResponseWriter.WriteAsync(context, healthStatus, context.RequestAborted);

                _logger.MiddlewareExecuted(GetType());

                return;
            }

            await _next(context);
        }
    }
}