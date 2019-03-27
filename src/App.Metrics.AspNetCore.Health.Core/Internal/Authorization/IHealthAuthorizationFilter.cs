// <copyright file="IHealthAuthorizationFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Health
{
  public interface IHealthAuthorizationFilter
  {
    bool Authorized(HttpContext context, CancellationToken token = default);
  }
}