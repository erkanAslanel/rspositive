using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
               

                options.UserinfoEndpointPath = "/api/v1/me";
                        options.TokenEndpointPath = "/api/v1/token";
                          options.AuthorizationEndpointPath = "/authorize/";
                          options.UseSlidingExpiration = false; // False means that new Refresh tokens aren't issued. Our implementation will be doing a no-expiry refresh, and this is one part of it.
                           options.AllowInsecureHttp = true; // ONLY FOR TESTING
                         options.AccessTokenLifetime = TimeSpan.FromHours(1); // An access token is valid for an hour - after that, a new one must be requested.
                             options.RefreshTokenLifetime = TimeSpan.FromDays(365 * 1000); //NOTE - Later versions of the ASOS library support `TimeSpan?` for these lifetime fields, meaning no expiration. 
                                                                                             // The version we are using does not, so a long running expiration of one thousand years will suffice.
             options.AuthorizationCodeLifetime = TimeSpan.FromSeconds(60);
                             options.IdentityTokenLifetime = options.AccessTokenLifetime;
                             options.ProviderType = typeof(SimpleAuthorizationServerProvider);
            });

            services.AddScoped<SimpleAuthorizationServerProvider>();
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
