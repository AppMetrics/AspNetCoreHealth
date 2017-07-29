// <copyright file="MiddlewareHealthChecksAppMetricsBuilderExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using App.Metrics.AspNetCore.Health.Options;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            var options = new AppMetricsAspNetHealthOptions();
            var provider = SetupServicesAndConfiguration();

            Action resolveOptions = () => { options = provider.GetRequiredService<AppMetricsAspNetHealthOptions>(); };

            resolveOptions.ShouldNotThrow();
            options.HealthEndpoint.Should().Be("/health-test");
            options.HealthEndpointEnabled.Should().Be(false);
        }

        [Fact]
        public void Can_override_settings_from_configuration()
        {
            var options = new AppMetricsAspNetHealthOptions();
            var provider = SetupServicesAndConfiguration(
                (o) =>
                {
                    o.HealthEndpointEnabled = true;
                });

            Action resolveOptions = () => { options = provider.GetRequiredService<AppMetricsAspNetHealthOptions>(); };

            resolveOptions.ShouldNotThrow();
            options.HealthEndpointEnabled.Should().Be(true);
        }

        private IServiceProvider SetupServicesAndConfiguration(
            Action<AppMetricsAspNetHealthOptions> setupHealthAction = null)
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            services.AddOptions();

#pragma warning disable CS0612
            services.AddHealth(_startupAssemblyName);
#pragma warning restore CS0612

            if (setupHealthAction == null)
            {
                services.AddHealthCheckMiddleware(configuration.GetSection("AppMetricsAspNetHealthOptions"));
            }
            else
            {
                services.AddHealthCheckMiddleware(configuration.GetSection("AppMetricsAspNetHealthOptions"), setupHealthAction);
            }

            return services.BuildServiceProvider();
        }
    }
}