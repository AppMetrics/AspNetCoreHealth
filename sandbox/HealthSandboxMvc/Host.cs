// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HealthSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseHealth()
                   .UseStartup<Startup>()
                   .Build();

        public static void Main(string[] args) { BuildWebHost(args).Run(); }
    }
}