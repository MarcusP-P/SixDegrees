using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SixDegrees.Server.Data;
using SixDegrees.Server.Models;

namespace SixDegrees.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseSqlServer(
                      this.Configuration.GetConnectionString("DefaultConnection")));

            _ = services.AddDatabaseDeveloperPageExceptionFilter();

            _ = services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            _ = services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            _ = services.AddAuthentication()
                .AddIdentityServerJwt();

            _ = services.AddControllersWithViews();
            _ = services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
                _ = app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                _ = app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                _ = app.UseHsts();
            }

            _ = app.UseHttpsRedirection();
            _ = app.UseBlazorFrameworkFiles();
            _ = app.UseStaticFiles();

            _ = app.UseRouting();

            _ = app.UseIdentityServer();
            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.UseEndpoints(endpoints =>
              {
                  _ = endpoints.MapRazorPages();
                  _ = endpoints.MapControllers();
                  _ = endpoints.MapFallbackToFile("index.html");
              });
        }
    }
}
