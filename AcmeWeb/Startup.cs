﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcmeWeb.Dal;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PoweredSoft.Data.EntityFrameworkCore;
using PoweredSoft.DynamicQuery;
using PoweredSoft.DynamicQuery.AspNetCore;
using PoweredSoft.DynamicQuery.Core;
using Swashbuckle.AspNetCore.Swagger;
using PoweredSoft.DynamicQuery.AspNetCore.NewtonsoftJson;

namespace AcmeWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AcmeContext>(options =>
            {
                options.UseInMemoryDatabase("AcmeWebSample");
            });

            services.AddPoweredSoftEntityFrameworkCoreDataServices();

            services.AddTransient<IDynamicControllerResourceProvider, AcmeResourceProvider>();

            var minimumDependencyServiceProvider = services.BuildServiceProvider();

            services.AddCors();

            services.AddTransient<IValidator<Customer>, CustomerValidator>(); 

            services
                .AddMvc(o =>
                {
                    o.Conventions.Add(new DynamicControllerRouteConvention(minimumDependencyServiceProvider));
                })
                .AddPoweredSoftJsonNetDynamicQuery()
                .ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(new DynamicControllerFeatureProvider(minimumDependencyServiceProvider));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "spa/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }

            UpdateDatabase(app);

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            app.UseCors(t => t.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseRouting(); 
            
            app.UseEndpoints(o =>
            {
                o.MapControllers();
            });
            
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "spa";
                if (env.EnvironmentName == "Development")
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); //(npmScript: "start");
                }
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AcmeContext>())
                {
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}
