// <copyright file="HealthAspNetCoreHealthAspNetCoreOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Health.Core.Internal
{
    public class HealthAspNetCoreHealthAspNetCoreOptionsSetup : IConfigureOptions<HealthAspNetCoreOptions>
    {
        /// <inheritdoc />
        public void Configure(HealthAspNetCoreOptions options)
        {
        }
    }
}