using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.Multitenant.Data;
using EFCore.Multitenant.Data.Interceptors;
using EFCore.Multitenant.Data.ModelFactory;
using EFCore.Multitenant.Domain;
using EFCore.Multitenant.Extensions;
using EFCore.Multitenant.Middlewares;
using EFCore.Multitenant.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace EFCore.Multitenant
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
            services.AddScoped<TenantData>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFCore.Multitenant", Version = "v1" });
            });

            //Estratégia 1 - Identificador na tabela
            /*
            services.AddScoped<StrategySchemaInterceptor>();

            services.AddDbContext<ApplicationContext>(optionsBuilder => optionsBuilder.UseSqlServer("Data Source=DESKTOP-B76722G\\SQLEXPRESS; Initial Catalog=Multitenant; User ID=developer; Password=dev*10; Integrated Security=True; Persist Security Info=False; Pooling=False; MultipleActiveResultSets=False; Encrypt=False; Trusted_Connection=False")
                                                                                      .LogTo(Console.WriteLine)
                                                                                      .EnableSensitiveDataLogging());
            */

            //Estratégia 2 - Schema
            /*
            services.AddDbContext<ApplicationContext>((provider, optionsBuilder) => 
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-B76722G\\SQLEXPRESS; Initial Catalog=Multitenant; User ID=developer; Password=dev*10; Integrated Security=True; Persist Security Info=False; Pooling=False; MultipleActiveResultSets=False; Encrypt=False; Trusted_Connection=False")
                              .LogTo(Console.WriteLine)
                              .ReplaceService<IModelCacheKeyFactory, StrategySchemaModelCacheKey>()
                              .EnableSensitiveDataLogging();

                //var interceptor = provider.GetRequiredService<StrategySchemaInterceptor>();

                //optionsBuilder.AddInterceptors(interceptor);
            });
            */

            //Estratégia 3 - Banco de dados
            services.AddHttpContextAccessor();

            services.AddScoped<ApplicationContext>(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

                var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;
                var tenantId = httpContext?.GetTenantId();

                //var connectionString = Configuration.GetConnectionString(tenantId);
                var connectionString = Configuration.GetConnectionString("custom")
                                                    .Replace("_DATABASE_", tenantId);

                optionsBuilder.UseSqlServer(connectionString)
                              .LogTo(Console.WriteLine)
                              .EnableSensitiveDataLogging();

                return new ApplicationContext(optionsBuilder.Options);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EFCore.Multitenant v1"));
            }

            //DatabaseInitialize(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseMiddleware<TenantMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //private void DatabaseInitialize(IApplicationBuilder app)
        //{
        //    using ApplicationContext db = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();

        //    db.Database.EnsureDeleted();
        //    db.Database.EnsureCreated();

        //    for (var index = 1; index <= 5; index++)
        //    {
        //        db.People.Add(new Person { Name = $"Person {index}" });
        //        db.Products.Add(new Product { Description = $"Product {index}" });
        //    }

        //    db.SaveChanges();
        //}
    }
}
