// <copyright file="AppMetricsAspNetCoreHealthOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Health.Core.Internal
{
    public class AppMetricsAspNetCoreHealthOptionsSetup : IConfigureOptions<AppMetricsAspNetHealthOptions>
    {
        /// <inheritdoc />
        public void Configure(AppMetricsAspNetHealthOptions options)
        {
        }
    }
}