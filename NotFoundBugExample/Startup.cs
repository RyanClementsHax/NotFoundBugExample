using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace NotFoundBugExample
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NotFoundBugExample", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            })
            .Configure<ExceptionHandlerOptions>(options =>
            {
                options.AllowStatusCode404Response = true;
                options.ExceptionHandler = async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var ex = errorFeature.Error;

                    context.Response.StatusCode = ex switch
                    {
                        DomainException domainEx => StatusCodes.Status400BadRequest,
                        NotFoundException notFoundEx => StatusCodes.Status404NotFound, // if you change this status code to anything else, it will be fine
                        _ => StatusCodes.Status500InternalServerError,
                    };

                    await context.Response.WriteAsync("There was an error caught in the exception handler lambda");
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NotFoundBugExample v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseExceptionHandler();

            // This exception handler works in all cases
            //app.Use(async (context, next) =>
            //{
            //    try
            //    {
            //        await next();
            //    }
            //    catch (Exception ex)
            //    {
            //        context.Response.StatusCode = ex switch
            //        {
            //            DomainException => StatusCodes.Status400BadRequest,
            //            // if you change this status code to anything else, it will be fine
            //            NotFoundException => StatusCodes.Status404NotFound,
            //            _ => StatusCodes.Status500InternalServerError,
            //        };

            //        await context.Response.WriteAsync("There was an error caught in the exception handler lambda");
            //    }
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
