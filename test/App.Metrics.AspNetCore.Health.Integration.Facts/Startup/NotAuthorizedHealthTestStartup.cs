// <copyright file="NotAuthorizedHealthTestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.Health;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Startup
{
  // ReSharper disable ClassNeverInstantiated.Global
  public class NotAuthorizedHealthTestStartup : TestStartup
    // ReSharper restore ClassNeverInstantiated.Global
  {
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      app.UseHealthEndpoint();

      SetupAppBuilder(app, env, loggerFactory);
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<IHealthAuthorizationFilter, NotAuthorizedHealthAuthorizationFilter>();

      var appMetricsMiddlewareHealthCheckOptions = new HealthEndpointsOptions();

      SetupServices(
        services,
        appMetricsMiddlewareHealthCheckOptions,
        healthChecks: new[] { HealthCheckResult.Healthy() });
    }
  }
}