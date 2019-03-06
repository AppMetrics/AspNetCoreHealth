// <copyright file="HealthCheckEndpointAuthorizationMiddleware.cs" company="Allan Hardy">
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
  public class HealthCheckEndpointAuthorizationMiddleware : IClassFixture<StartupTestFixture<NotAuthorizedHealthTestStartup>>
  {
    public HealthCheckEndpointAuthorizationMiddleware(StartupTestFixture<NotAuthorizedHealthTestStartup> fixture)
    {
      Client = fixture.Client;
    }

    private HttpClient Client { get; }

    [Fact]
    public async Task Returns_correct_response_headers_not_authorized()
    {
      var result = await Client.GetAsync("/health");

      result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
  }
}