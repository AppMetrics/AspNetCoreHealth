// <copyright file="AppMetricsHealthJsonOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace App.Metrics.AspNetCore.Health.Formatters.Json
{
    // TODO: Move this to health core
    public class AppMetricsHealthJsonOptions
    {
        public JsonSerializerSettings SerializerSettings { get; } =
            DefaultJsonSerializerSettings.CreateSerializerSettings();
    }
}
