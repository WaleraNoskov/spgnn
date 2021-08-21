using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using spgnn.DAL;
using spgnn.DAL.Repositories;
using spgnn.Models;

namespace spgnn
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
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            var connectionString = Configuration.GetSection("ConnectionStrings:spgnn").Value;
            connectionString = connectionString.Replace("/this", Directory.GetCurrentDirectory());
            services.AddDbContext<SpgnndbContext>(options => options.UseSqlite(connectionString).UseLazyLoadingProxies());
            
            var connectionStringInfoArticles = Configuration.GetSection("ConnectionStrings:infoArticles").Value;
            connectionStringInfoArticles = connectionStringInfoArticles.Replace("/this", Directory.GetCurrentDirectory());
            services.AddDbContext<InfoArticlesContext>(options => options.UseSqlite(connectionStringInfoArticles).UseLazyLoadingProxies());

            var connectionStringIdentity = Configuration.GetSection("ConnectionStrings:identity").Value;
            connectionStringIdentity = connectionString.Replace("/this", Directory.GetCurrentDirectory());
            services.AddDbContext<IdentityContext>(options => options.UseSqlite(connectionStringIdentity).UseLazyLoadingProxies());
            
            var connectionStringSections = Configuration.GetSection("ConnectionStrings:sections").Value;
            connectionStringSections = connectionString.Replace("/this", Directory.GetCurrentDirectory());
            services.AddDbContext<SectionsContext>(options => options.UseSqlite(connectionStringSections).UseLazyLoadingProxies());
            

            services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                    }
                )
                .AddEntityFrameworkStores<IdentityContext>();
            services.AddTransient<IRepositoryBase<Article>, RepositoryBase<Article>>();
            services.AddTransient<IInfoArticleRepository<Article>, InfoArticleRepository<Article>>();
            services.AddTransient<ISectionsRepository<Article>, SectionsRepository<Article>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
