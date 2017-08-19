// <copyright file="HealthWebHostOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.Health;

namespace App.Metrics.AspNetCore.Health
{
    /// <summary>
    ///     Provides programmatic configuration for health and health endpoints in the App Metrics framework.
    /// </summary>
    public class HealthWebHostOptions
    {
        public HealthWebHostOptions()
        {
            HealthOptions = options => { };
            EndpointOptions = options => { };
        }

        /// <summary>
        ///     Gets or sets <see cref="Action{HealthOptions}" /> to configure the provided <see cref="HealthOptions" />.
        /// </summary>
        public Action<HealthOptions> HealthOptions { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="Action{EndpointOptions}" /> to configure the provided <see cref="EndpointOptions" />.
        /// </summary>
        public Action<HealthEndpointOptions> EndpointOptions { get; set; }
    }
}