// <copyright file="HealthStartupFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace HealthSandboxMvc
{
    public class HealthStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return AddHealth;

            void AddHealth(IApplicationBuilder builder)
            {
                builder.UseHealthEndpoint();

                next(builder);
            }
        }
    }
}