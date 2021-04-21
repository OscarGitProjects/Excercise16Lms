using Lms.Core.Repositories;
using Lms.Data.Data;
using Lms.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Lms.Api
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
            services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", 
                    new OpenApiInfo 
                    { 
                        Title = "Lms.Api", 
                        Version = "1.0", 
                        Description = "Uppgift 16. Lmi",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "oscarandersson2@hotmail.com",
                            Name = "Oscar Andersson",
                            Url = new Uri("https://www.microsoft.com")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "My licens",
                            Url = new Uri("https://www.microsoft.com")
                        }
                    });

                // Get the xml comments file
                var xmlCommentsFileApi = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentFullPathApi = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileApi);
                var xmlCommentFullPathCore = Path.Combine(AppContext.BaseDirectory, "Lms.Core.xml");
                var xmlCommentFullPathData = Path.Combine(AppContext.BaseDirectory, "Lms.Data.xml");

                c.IncludeXmlComments(xmlCommentFullPathApi);
                c.IncludeXmlComments(xmlCommentFullPathCore);
                c.IncludeXmlComments(xmlCommentFullPathData);
            });

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("Excercise16LmsHome")));

            //options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext")));

            services.AddAutoMapper(typeof(MapperProfile));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddSwaggerGen(setUpAction => 
            //{
            //    setUpAction.SwaggerDoc("LibraryOpenAPISpecification", new Microsoft.OpenApi.Models.OpenApiInfo()
            //    { 
            //        Title = "LMS API",
            //        Version = "1.0",
            //        Description = "LMS API"
            //    });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lms.Api v1.0"));
            }
            else
            {
                // Error messages for production
                app.UseExceptionHandler(appBuilder => 
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happend. Try again later");
                    });
                });
            }

            app.UseHttpsRedirection();

            //app.UseSwagger();

            //app.UseSwaggerUI(setUpAction => 
            //{
            //    setUpAction.SwaggerEndpoint("/swagger/LibraryOpenAPISpecification/swagger.json", "LMS Api");
            //    setUpAction.RoutePrefix = "";
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}