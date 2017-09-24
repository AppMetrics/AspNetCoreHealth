// <copyright file="HealthEndpointOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Formatters;

namespace App.Metrics.AspNetCore.Health.Endpoints
{
    public class HealthEndpointOptions
    {
        public HealthEndpointOptions() { HealthEndpointEnabled = true; }

        /// <summary>
        ///     Gets or sets a value indicating whether [health endpoint should be enabled], if disabled endpoint responds with
        ///     404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [health endpoint is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool HealthEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IHealthOutputFormatter" /> used to write the health status when the health endpoint is
        ///     requested.
        /// </summary>
        /// <value>
        ///     The <see cref="IHealthOutputFormatter" /> used to write metrics.
        /// </value>
        public IHealthOutputFormatter HealthEndpointOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the timeout when reading health checks via the health endpoint.
        /// </summary>
        public TimeSpan Timeout { get; set; }
    }
}