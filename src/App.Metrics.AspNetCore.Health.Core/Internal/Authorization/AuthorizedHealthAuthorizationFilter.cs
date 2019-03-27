// <copyright file="AuthorizedHealthAuthorizationFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Health
{
  public class AuthorizedHealthAuthorizationFilter : IHealthAuthorizationFilter
  {
    /// <inheritdoc />
    public bool Authorized(HttpContext context, CancellationToken token = default) { return true; }
  }
}
