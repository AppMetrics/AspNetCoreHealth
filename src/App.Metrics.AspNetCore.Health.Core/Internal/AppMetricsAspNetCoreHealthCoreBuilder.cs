// <copyright file="AppMetricsAspNetCoreHealthCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Health.Core.Internal
{
    public class AppMetricsAspNetCoreHealthCoreBuilder : IAppMetricsAspNetCoreHealthCoreBuilder
    {
        public AppMetricsAspNetCoreHealthCoreBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}
