// <copyright file="HealthAspNetCoreOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core.Internal;

namespace App.Metrics.AspNetCore.Health.Core
{
    public class HealthAspNetCoreOptions
    {
        public HealthAspNetCoreOptions()
        {
            HealthEndpointEnabled = true;
        }

        /// <summary>
        ///     Gets or sets the health endpoint, defaults to /health.
        /// </summary>
        /// <value>
        ///     The health endpoint.
        /// </value>
        public string HealthEndpoint { get; set; } = HealthMiddlewareConstants.DefaultRoutePaths.HealthEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets a value indicating whether [health endpoint should be enabled], if disabled endpoint responds with
        ///     404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [health endpoint is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool HealthEndpointEnabled { get; set; }
    }
}