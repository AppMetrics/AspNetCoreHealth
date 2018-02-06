// <copyright file="HealthMiddlewareConstants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.AspNetCore.Health.Endpoints.Internal
{
    internal static class HealthMiddlewareConstants
    {
        public static class DefaultRoutePaths
        {
            public const string HealthEndpoint = "/health";
            public const string PingEndpoint = "/ping";
        }
    }
}