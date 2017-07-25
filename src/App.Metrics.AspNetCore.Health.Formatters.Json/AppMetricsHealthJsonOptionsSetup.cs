// <copyright file="AppMetricsHealthJsonOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Health;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace App.Metrics.AspNetCore.Health.Formatters.Json
{
    public class AppMetricsHealthJsonOptionsSetup : IConfigureOptions<AppMetricsHealthOptions>
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public AppMetricsHealthJsonOptionsSetup(IOptions<AppMetricsHealthJsonOptions> jsonOptions)
        {
            _jsonSerializerSettings = jsonOptions.Value.SerializerSettings;
        }

        public void Configure(AppMetricsHealthOptions options)
        {
        }
    }
}
