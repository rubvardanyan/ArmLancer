﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmLancer.API.Utils.Extensions;
using ArmLancer.API.Utils.Settings;
using ArmLancer.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace ArmLancer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AuthSettings = configuration.GetSettings<AuthSettings>();
        }

        public IConfiguration Configuration { get; }
        public AuthSettings AuthSettings { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "ArmLancer API",
                        Version = "v1",
                        Contact = new Contact
                        {
                            Email = "Merujan99@gmail.com",
                            Name = "Meruzhan Hovhannisyan",
                            Url = null
                        }
                    }
                 );
            });
            
            services.RegisterCors("CorsPolicy");
            
            services.RegisterAuth(AuthSettings);

            services.AddDbContext<ArmLancerDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Seed Data
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ArmLancerDbContext>().EnsureSeedData();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>  
            {  
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArmLancer API v1");  
            });
            
            app.UseCors("CorsPolicy");
            
            app.UseAuthentication();
            
            app.UseMvc();
        }
    }
}