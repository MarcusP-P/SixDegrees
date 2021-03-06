// <copyright file="Startup.cs" company="Marcus Pallinger">
// Copyright (c) 2019 Marcus Pallinger. All rights reserved.
// Licensed under the BSD 2-clause license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
// Contains the Startup class, which configures DI, and the middleware.
// </summary>

namespace SixDegrees
{
    using System.Diagnostics.Contracts;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SixDegrees.Model;

    /// <summary>
    /// Startup the web services.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration supplied by DI.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The global configuration of the server.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddControllersWithViews();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            // Register the Swagger services
            _ = services.AddOpenApiDocument();

            _ = services.AddDbContext<MoviesContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("MoviesDatabase")));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application Builder.</param>
        /// <param name="env">Hosting environment.</param>
#pragma warning disable SA1204 // Static elements should appear before instance elements
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore SA1204 // Static elements should appear before instance elements
        {
            Contract.Requires(app != null);

            using (var scope = app!.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetService<MoviesContext>())
            {
                context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }
            else
            {
                _ = app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                _ = app.UseHsts();
            }

            _ = app.UseHttpsRedirection();
            _ = app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            _ = app.UseRouting();

            // Register the Swagger generator and the Swagger UI middlewares
            _ = app.UseOpenApi();
            _ = app.UseSwaggerUi3();

            _ = app.UseEndpoints(endpoints =>
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}"));

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
