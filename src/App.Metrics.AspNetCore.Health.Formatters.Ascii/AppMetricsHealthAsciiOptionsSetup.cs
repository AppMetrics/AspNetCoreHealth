// <copyright file="AppMetricsHealthAsciiOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Health;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Health.Formatters.Ascii
{
    public class AppMetricsHealthAsciiOptionsSetup : IConfigureOptions<AppMetricsHealthOptions>
    {
        private readonly IOptions<AppMetricsHealthAsciiOptions> _asciiOptions;

        public AppMetricsHealthAsciiOptionsSetup(IOptions<AppMetricsHealthAsciiOptions> asciiOptions) { _asciiOptions = asciiOptions; }

        public void Configure(AppMetricsHealthOptions options)
        {
        }
    }
}
