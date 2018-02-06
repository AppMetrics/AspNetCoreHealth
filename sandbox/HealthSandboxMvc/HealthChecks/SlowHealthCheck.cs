// <copyright file="SlowHealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace HealthSandboxMvc.HealthChecks
{
    public class SlowHealthCheck : HealthCheck
    {
        public SlowHealthCheck()
            : base("Slow Health Check")
        {
        }

        /// <inheritdoc />
        protected override async ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);

            if (DateTime.UtcNow.Second <= 20)
            {
                return HealthCheckResult.Degraded();
            }

            if (DateTime.UtcNow.Second >= 40)
            {
                return HealthCheckResult.Unhealthy();
            }

            return HealthCheckResult.Healthy();
        }
    }
}