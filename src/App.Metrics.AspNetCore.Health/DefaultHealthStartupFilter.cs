// <copyright file="DefaultHealthStartupFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace App.Metrics.AspNetCore.Health
{
    /// <summary>
    /// Inserts the App Metrics Health Middleware at the request pipeline
    /// </summary>
    public class DefaultHealthStartupFilter : IStartupFilter
    {
        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return AddAllHealthEndpoints;

            void AddAllHealthEndpoints(IApplicationBuilder app)
            {
                app.UseHealthAllEndpoints();
                app.UsePingEndpoint();

                next(app);
            }
        }
    }
}