﻿// <copyright file="IHealthResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Health
{
    public interface IHealthResponseWriter
    {
        Task WriteAsync(HttpContext context, HealthStatus healthStatus, CancellationToken token = default);
    }
}