// <copyright file="RequestHeaderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Formatters;
using Microsoft.Net.Http.Headers;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http.Headers
    // ReSharper restore CheckNamespace
{
    public static class RequestHeaderExtensions
    {
        public static TFormatter ResolveFormatter<TFormatter>(
            this RequestHeaders headers,
            TFormatter defaultFormatter,
            Func<HealthMediaTypeValue, TFormatter> resolveOutputFormatter)
        {
            if (headers.Accept == null)
            {
                return defaultFormatter;
            }

            var formatter = defaultFormatter;

            foreach (var accept in headers.Accept)
            {
                var metricsMediaTypeValue = accept.ToHealthMediaType();

                if (metricsMediaTypeValue != default)
                {
                    formatter = resolveOutputFormatter(metricsMediaTypeValue);
                }

                if (formatter != null)
                {
                    return formatter;
                }
            }

            return defaultFormatter;
        }
    }
}
