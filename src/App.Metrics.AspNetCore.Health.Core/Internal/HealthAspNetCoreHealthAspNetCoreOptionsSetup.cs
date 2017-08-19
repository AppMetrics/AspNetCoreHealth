// <copyright file="HealthAspNetCoreHealthAspNetCoreOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Health.Core.Internal
{
    /// <summary>
    ///     Sets up default health endpoint options for <see cref="HealthEndpointOptions"/>.
    /// </summary>
    public class HealthAspNetCoreHealthAspNetCoreOptionsSetup : IConfigureOptions<HealthEndpointOptions>
    {
        /// <inheritdoc />
        public void Configure(HealthEndpointOptions options)
        {
        }
    }
}