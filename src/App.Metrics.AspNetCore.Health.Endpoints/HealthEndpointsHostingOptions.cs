// <copyright file="HealthEndpointsHostingOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Endpoints.Internal;

namespace App.Metrics.AspNetCore.Health.Endpoints
{
    /// <summary>
    ///     Provides programmatic configuration for metrics endpoints hosting in the App Metrics framework.
    /// </summary>
    public class HealthEndpointsHostingOptions
    {
        /// <summary>
        ///     Gets or sets the health endpoint, defaults to /health.
        /// </summary>
        /// <value>
        ///     The health endpoint.
        /// </value>
        public string HealthEndpoint { get; set; } = HealthMiddlewareConstants.DefaultRoutePaths.HealthEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets the port to host the health endpoint.
        /// </summary>
        public int? HealthEndpointPort { get; set; }
    }
}