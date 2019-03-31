using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.AspNetCore.Mvc;
using LiftService.Domain.Model;
using LiftService.Controller.Controllers;


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
                .AddApplicationPart(typeof(LiftController).GetTypeInfo().Assembly).AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddDbContext<GymtContext>(opt => opt.UseInMemoryDatabase("Lift"));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = typeof(Startup).Namespace, Version = "v1" });
            });

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

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
        
        private void InitializeContainer(IApplicationBuilder app)
        {
            container.RegisterMvcControllers(app);

            // Add application services. For instance:
            // container.Register<IUserService, UserService>(Lifestyle.Scoped);

            /*
             * Cross-Wiring will enable SimpleInjector to inject the
             * DbContext registered on the built-in IServiceCollection.
             */
            container.AutoCrossWireAspNetComponents(app);

            container.Verify();
        }
        
    }
}
