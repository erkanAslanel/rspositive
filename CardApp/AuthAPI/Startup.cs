using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthAPI.Repository;
using AuthAPI.Service;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthAPI
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
            services.AddMvc();

            services.AddAuthentication().AddOpenIdConnectServer(options =>
            { 
                options.UserinfoEndpointPath = "/api/account/me";
                options.TokenEndpointPath = "/api/token";
                options.AuthorizationEndpointPath = "/authorize/";
                options.UseSlidingExpiration = true;
                options.AllowInsecureHttp = true;
                options.AccessTokenLifetime = TimeSpan.FromHours(1);
                options.RefreshTokenLifetime = TimeSpan.FromDays(365 * 1000);
                options.AuthorizationCodeLifetime = TimeSpan.FromSeconds(60);
                options.IdentityTokenLifetime = options.AccessTokenLifetime;
                options.ProviderType = typeof(CustomAuthorizationServerProvider);
            });

            services.AddScoped<CustomAuthorizationServerProvider>();
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IAccountService, AccountService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();


        }
    }
}
