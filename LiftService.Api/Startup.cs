using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.AspNetCore.Mvc;
using AuthService.Controller.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Logging;

namespace LiftService.Api
{
    public class Startup
    {
        private Container container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddApiExplorer()
                .AddAuthorization()
                .AddFormatterMappings()
                .AddJsonFormatters()
                .AddCors()
                .AddApplicationPart(Assembly.Load("LiftService.Controller")).AddControllersAsServices()
                .AddApplicationPart(Assembly.Load("AuthService.Controller")).AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = typeof(Startup).Namespace, Version = "v1.0" });

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new ApiKeyScheme
                {
                    Description = "Please enter `Bearer <token>`",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });

            services.AddCognitoIdentity();

            services
                .AddAuthentication(c =>
                {
                    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Audience = Configuration.GetAWSCognitoClientOptions().UserPoolClientId;
                    options.Authority = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_zpNZcYNEl";
                });

            services.AddHealthChecks();

            IntegrateSimpleInjector(services);
        }
        
        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddHttpContextAccessor();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API v1.0");
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseHealthChecks("/healthcheck");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
        
        private void InitializeContainer(IApplicationBuilder app)
        {
            container.RegisterMvcControllers(app);

            // Add application services. For instance:
            container.Register<IAuthService, AuthService.Domain.Services.AuthService>(Lifestyle.Scoped);

            /*
             * Cross-Wiring will enable SimpleInjector to inject a
             * DbContext registered on the built-in IServiceCollection.
             */
            container.AutoCrossWireAspNetComponents(app);

            container.Verify();
        }
        
    }
}
