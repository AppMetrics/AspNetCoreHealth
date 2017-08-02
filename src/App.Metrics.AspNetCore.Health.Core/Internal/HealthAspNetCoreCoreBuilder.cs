// <copyright file="HealthAspNetCoreCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Health.Core.Internal
{
    public class HealthAspNetCoreCoreBuilder : IHealthAspNetCoreCoreBuilder
    {
        public HealthAspNetCoreCoreBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}
