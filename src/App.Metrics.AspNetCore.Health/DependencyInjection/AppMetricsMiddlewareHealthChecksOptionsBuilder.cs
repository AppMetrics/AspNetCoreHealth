// <copyright file="AppMetricsMiddlewareHealthChecksOptionsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public sealed class AppMetricsMiddlewareHealthChecksOptionsBuilder : IAppMetricsMiddlewareHealthChecksOptionsBuilder
    {
        public AppMetricsMiddlewareHealthChecksOptionsBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}