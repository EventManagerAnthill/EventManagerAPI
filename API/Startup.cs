using API.Middlewares.Autentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Study.EventManager.Services;
using ContainerConfiguration = Study.EventManager.Services.ContainerConfiguration;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;
using Azure.Storage.Blobs;

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
     
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGenNewtonsoftSupport();          
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "EventManager API",
                    Description = "API documentation of 'EventManager API'",
                    TermsOfService = new Uri("https://steventmanagerdev01.z13.web.core.windows.net/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Kirill Kuznetsov" + " " + @"GitHub Repository",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/KuznetsovKirill/EventManager")
                    }
                });

                /*var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);*/
            });

            services.AddScoped(_ => {
                IConfigurationSection azureConnectionString =
                     Configuration.GetSection("Azure");
                return new BlobServiceClient(azureConnectionString["AzureStorage"]);
            });

            var authOptions = Configuration.GetSection("AuthOptions").Get<AuthOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
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
                })
            .AddGoogle(options =>
             {
                 IConfigurationSection googleAuthNSection =
                     Configuration.GetSection("Authentication:Google");

                 options.ClientId = googleAuthNSection["ClientId"];
                 options.ClientSecret = googleAuthNSection["ClientSecret"];                                 
                 options.Scope.Add("email");
             })
            .AddCookie()
            .AddFacebook(facebookOptions =>
            {
                IConfigurationSection facebookAuthNSection =
                     Configuration.GetSection("Authentication:FaceBook");
                facebookOptions.AppId = facebookAuthNSection["AppId"];
                facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];
                facebookOptions.CallbackPath = "/api/user/facebook-response";
            });
        
            ContainerConfiguration.Configure(services, _settings);
        }
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {         
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }   
    }
}
