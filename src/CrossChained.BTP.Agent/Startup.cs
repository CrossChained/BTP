using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrossChained.BTP.Agent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = this.Configuration.GetSection("APICFG_CONNECTIONSTRING").Value;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = Configuration.GetConnectionString("DefaultConnection");
            }

            services.AddDbContext<Models.DbModel>(
                builder => builder.UseNpgsql(connectionString,
                        options =>
                        {
                            options.SetPostgresVersion(new System.Version(10, 0));
                            options.CommandTimeout(5 * 60);
                        }));

            services.AddCors();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "True DEX", Version = "v1" });
                c.EnableAnnotations();
                c.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(options =>
                    options.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            }

            app.UseWebSockets();
            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trusty API V1");
                c.RoutePrefix = "api";
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws/" || context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (var scope = app.ApplicationServices.CreateScope())
                        {
                            try
                            {
                                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                                var processor = scope.ServiceProvider.GetService<API.IWebSocketAPI>();
                                await processor.Start(webSocket);
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });
        }
    }
}
