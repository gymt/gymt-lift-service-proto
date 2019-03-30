using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SimpleInjector;
using SimpleInjector.Lifestyles;
using Microsoft.EntityFrameworkCore;
using SimpleInjector.Integration.AspNetCore.Mvc;
using System.Reflection;
using LiftService.Domain.Model;

namespace LiftService.Program
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
            var builder = services.AddMvcCore();
            builder.AddAuthorization();
            builder.AddFormatterMappings();
            builder.AddJsonFormatters();
            builder.AddCors();
            builder.AddApplicationPart(Assembly.Load("LiftService.Controller")).AddControllersAsServices();
            builder.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the database context
            services.AddDbContext<LiftContext>(opt => opt.UseInMemoryDatabase("Lift"));

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
                app.UseDeveloperExceptionPage();
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

            container.AutoCrossWireAspNetComponents(app);
        }
        
    }
}
