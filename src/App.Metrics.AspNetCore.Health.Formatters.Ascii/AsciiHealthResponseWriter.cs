// <copyright file="AsciiHealthResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Formatting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Health.Formatters.Ascii
{
    public class AsciiHealthResponseWriter : IHealthResponseWriter
    {
        private readonly AppMetricsHealthAsciiOptions _options;

        public AsciiHealthResponseWriter(IOptions<AppMetricsHealthAsciiOptions> options) { _options = options.Value; }

        /// <inheritdoc />
        public string ContentType => "text/plain; app.metrics=vnd.app.metrics.v1.health;";

        public Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default(CancellationToken))
        {
            var payloadBuilder = new AsciiHealthStatusPayloadBuilder();

            HealthStatusPayloadFormatter.Build(healthStatus, payloadBuilder);

            return context.Response.WriteAsync(payloadBuilder.PayloadFormatted(), token);
        }
    }
}