using Blog.Models;
using Blog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blog
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>(options => 
            {
                options.Password.RequiredLength = 8;  
                options.Password.RequireNonAlphanumeric = false;  
                options.Password.RequireLowercase = false; 
                options.Password.RequireUppercase = false; 
                options.Password.RequireDigit = false; 
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddTransient<ChartViewModelGenerator>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMvc(routes => 
            {
                routes.MapRoute(
                    name: "pagination",
                    template: "Articles/Page{PageId}",
                    defaults: new { Controller = "Articles", Action = "Index" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Articles}/{action=Index}");
            });
        }
    }
}
