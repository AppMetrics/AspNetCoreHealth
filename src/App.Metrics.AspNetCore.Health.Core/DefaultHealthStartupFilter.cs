// <copyright file="DefaultHealthStartupFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace App.Metrics.AspNetCore.Health.Core
{
    public class DefaultHealthStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return AddHealth;

            void AddHealth(IApplicationBuilder builder)
            {
                builder.UseHealth();

                next(builder);
            }
        }
    }
}