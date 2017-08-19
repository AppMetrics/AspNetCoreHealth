// <copyright file="MiddlewareHealthChecksAppMetricsBuilderExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Core;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.DependencyInjection
{
    public class MiddlewareHealthChecksAppMetricsBuilderExtensionsTests
    {
        private readonly string _startupAssemblyName;

        public MiddlewareHealthChecksAppMetricsBuilderExtensionsTests()
        {
            _startupAssemblyName = typeof(MiddlewareHealthChecksAppMetricsBuilderExtensionsTests).Assembly.GetName().Name;
        }

        [Fact]
        public void Can_load_settings_from_configuration()
        {
            var options = new HealthEndpointOptions();
            var provider = SetupServicesAndConfiguration();

            Action resolveOptions = () => { options = provider.GetRequiredService<IOptions<HealthEndpointOptions>>().Value; };

            resolveOptions.ShouldNotThrow();
            options.Endpoint.Should().Be("/health-test");
            options.Enabled.Should().Be(false);
        }

        [Fact]
        public void Can_override_settings_from_configuration()
        {
            var options = new HealthEndpointOptions();
            var provider = SetupServicesAndConfiguration(
                (o) => { o.Enabled = true; });

            Action resolveOptions = () => { options = provider.GetRequiredService<IOptions<HealthEndpointOptions>>().Value; };

            resolveOptions.ShouldNotThrow();
            options.Enabled.Should().Be(true);
        }

        private IServiceProvider SetupServicesAndConfiguration(
            Action<HealthEndpointOptions> setupHealthAction = null)
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            services.AddOptions();

#pragma warning disable CS0612
            var healthBuilder = services.AddHealth(_startupAssemblyName);
#pragma warning restore CS0612

            if (setupHealthAction == null)
            {
                healthBuilder.AddAspNetCoreHealth(configuration.GetSection(nameof(HealthEndpointOptions)));
            }
            else
            {
                healthBuilder.AddAspNetCoreHealth(configuration.GetSection(nameof(HealthEndpointOptions)), setupHealthAction);
            }

            return services.BuildServiceProvider();
        }
    }
}