// <copyright file="HealthEndpointOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core.Internal;
using App.Metrics.Health.Formatters;

namespace App.Metrics.AspNetCore.Health.Core
{
    public class HealthEndpointOptions
    {
        public HealthEndpointOptions() { Enabled = true; }

        /// <summary>
        ///     Gets or sets a value indicating whether [health endpoint should be enabled], if disabled endpoint responds with
        ///     404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [health endpoint is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }

        /// <summary>
        ///     Gets or sets the health endpoint, defaults to /health.
        /// </summary>
        /// <value>
        ///     The health endpoint.
        /// </value>
        public string Endpoint { get; set; } = HealthMiddlewareConstants.DefaultRoutePaths.HealthEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets the <see cref="IHealthOutputFormatter" /> used to write the health status when the health endpoint is
        ///     requested.
        /// </summary>
        /// <value>
        ///     The <see cref="IHealthOutputFormatter" /> used to write metrics.
        /// </value>
        public IHealthOutputFormatter EndpointOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the port to host the health endpoint.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        ///     Gets or sets the timeout when reading health checks via the health endpoint.
        /// </summary>
        public TimeSpan Timeout { get; set; }
    }
}