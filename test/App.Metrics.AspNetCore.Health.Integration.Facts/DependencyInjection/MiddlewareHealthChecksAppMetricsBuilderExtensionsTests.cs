// <copyright file="MiddlewareHealthChecksAppMetricsBuilderExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.Health;
using App.Metrics.Health.Extensions.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.DependencyInjection
{
    public class MiddlewareHealthChecksAppMetricsBuilderExtensionsTests
    {
        [Fact]
        public void Can_load_settings_from_configuration()
        {
            var endpointOptions = new HealthEndpointOptions();

            var provider = SetupServicesAndConfiguration();
            Action resolveEndpointsOptions = () => { endpointOptions = provider.GetRequiredService<IOptions<HealthEndpointOptions>>().Value; };

            resolveEndpointsOptions.ShouldNotThrow();

            endpointOptions.HealthEndpointEnabled.Should().Be(false);
        }

        [Fact]
        public void Can_override_settings_from_configuration()
        {
            var options = new HealthEndpointOptions();
            var provider = SetupServicesAndConfiguration(
                o =>
                {
                    o.HealthEndpointEnabled = true;
                    o.Timeout = TimeSpan.FromDays(1);
                });

            Action resolveOptions = () => { options = provider.GetRequiredService<IOptions<HealthEndpointOptions>>().Value; };

            resolveOptions.ShouldNotThrow();
            options.HealthEndpointEnabled.Should().Be(true);
            options.Timeout.Should().Be(TimeSpan.FromDays(1));
        }

        private IServiceProvider SetupServicesAndConfiguration(
            Action<HealthEndpointOptions> setupEndpointAction = null)
        {
            var services = new ServiceCollection();
            services.AddOptions();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("DependencyInjection/TestConfiguration/appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var healthBuilder = AppMetricsHealth.CreateDefaultBuilder()
                .Configuration.ReadFrom(configuration);
            services.AddHealth(healthBuilder);

            if (setupEndpointAction == null)
            {
                services.AddHealthEndpoints(configuration.GetSection(nameof(HealthEndpointOptions)));
            }
            else
            {
                services.AddHealthEndpoints(configuration.GetSection(nameof(HealthEndpointOptions)), setupEndpointAction);
            }

            return services.BuildServiceProvider();
        }
    }
}