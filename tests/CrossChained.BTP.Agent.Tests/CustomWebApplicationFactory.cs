using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrossChained.BTP.Agent.Tests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("test");
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    //.AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                var config = sp.GetService<IConfiguration>();
                //config["ASPNETCORE_ENVIRONMENT"] = "Test";
                config["APICFG_JWTKEY"] = "8bc7d970a73c455e9d43020abd907cbb1ba8d725b4fde54ae70a592aaf6df904";
                config["APICFG_JWTISSUER"] = "test";
                config["APICFG_JWTAUDIENCE"] = "http://localhost";
                config["APICFG_PUBLICURL"] = "http://localhost";

            });
        }
    }
}
