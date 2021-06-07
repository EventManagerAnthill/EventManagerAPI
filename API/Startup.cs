using API.Middlewares.Autentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Study.EventManager.Data;

using Study.EventManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContainerConfiguration = Study.EventManager.Services.ContainerConfiguration;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _settings = Configuration.Get<Settings>();
        }
         
        public IConfiguration Configuration { get; }

        private Settings _settings; 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();            
            var authOptions = Configuration.GetSection("AuthOptions").Get<AuthOptions>();

            

            services.AddAuthentication()
                  .AddJwtBearer(options =>
                  {
                      options.RequireHttpsMetadata = false;
                      options.TokenValidationParameters = new TokenValidationParameters
                      {                           
                            ValidateIssuer = true,                           
                            ValidIssuer = authOptions.Issuer,                       
                            ValidateAudience = true,
                            ValidAudience = authOptions.Audience,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(authOptions.SecretKey),
                            ValidateIssuerSigningKey = true,
                      };
                  });

            ContainerConfiguration.Configure(services, _settings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
