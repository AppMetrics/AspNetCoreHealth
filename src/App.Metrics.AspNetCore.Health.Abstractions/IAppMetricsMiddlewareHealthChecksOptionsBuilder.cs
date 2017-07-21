// <copyright file="IAppMetricsMiddlewareHealthChecksOptionsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Provides extensiblity on App Metrics middleware e.g. serialization options on supported endpoints.
    /// </summary>
    public interface IAppMetricsMiddlewareHealthChecksOptionsBuilder
    {
        IServiceCollection Services { get; }
    }
}