// <copyright file="HealthCheckEndpointMiddlewareTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Middleware
{
    public class HealthCheckEndpointMiddlewareTests : IClassFixture<StartupTestFixture<HealthyHealthTestStartup>>
    {
        public HealthCheckEndpointMiddlewareTests(StartupTestFixture<HealthyHealthTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task Returns_correct_response_headers()
        {
            var result = await Client.GetAsync("/health");

            result.Headers.CacheControl.NoCache.Should().Be(true);
            result.Headers.CacheControl.NoStore.Should().Be(true);
            result.Headers.CacheControl.MustRevalidate.Should().Be(true);
            result.Headers.Pragma.ToString().Should().Be("no-cache");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}