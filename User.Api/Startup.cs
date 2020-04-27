using System;
using AutoMapper;
using GreenPipes;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared.Identity;
using Shared.Persistence.MySql;
using User.Application.GetAllUsers;
using User.Application.Infrastructure;
using User.Persistence;

namespace User.Api
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
            var connectionString = Configuration["ConnectionString"];

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, builder =>
                {
                    builder.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));
            services.AddScoped<ISqlRepository<ApplicationUser>, UserRepository>();

            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.AddMediatR(typeof(GetAllUsersQuery).Assembly);

            services.AddMassTransit(x =>
            {
                //x.AddConsumer<DeleteChapterConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    // configure health checks for this bus instance
                    cfg.UseHealthCheck(provider);

                    cfg.Host("rabbitmq://localhost");

                    cfg.ReceiveEndpoint("delete-chapter", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));

                        //ep.ConfigureConsumer<DeleteChapterConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();

            var identityUrl = Configuration["IdentityUrl"];

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "You api title",
                    Version = "v1"
                });
            });


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API V1");
                c.OAuthClientId("SwaggerId");
                c.OAuthAppName("Swagger UI");
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
