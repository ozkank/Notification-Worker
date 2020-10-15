using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoticationIntegration.Infrastructure.Service;
using NotificationIntegration.API.Filters;
using NotificationIntegration.API.Middlewares;
using NotificationIntegration.API.Swagger;
using NotificationIntegration.Domain.Object;
using NotificationIntegration.Domain.Object.Options;
using NotificationIntegration.Domain.Services;
using NotificationIntegration.Domain.Services.Interfaces;
using NotificationIntegration.Infrastructure.Repositories;

namespace NotificationIntegration.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Services
            services.AddSingleton<IMessageQueueService, RabbitMQService>();
            services.AddSingleton(serviceProvider =>
            {
                var options = serviceProvider.GetService<IOptions<RabbitMQOptions>>();
                return new RabbitMQMessagingConnectionFactory(options.Value.ConnectionString);
            });
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<INoSqlRepository<NotificationDto>, MongoRepository<NotificationDto>>();

            services.AddHostedService<NotificationConsumerService>();
            #endregion

            #region Options

            services.AddOptions()
                .Configure<NotificationOptions>(Configuration.GetSection("Notification"))
                .Configure<RabbitMQOptions>(Configuration.GetSection("RabbitMQ"))
                .Configure<MongoDbOptions>(Configuration.GetSection("MongoDb"));
            #endregion

            services.AddControllers(setup =>
            {
                setup.Filters.Add<ActionTrackingFilter>();
            });
            services.EnableSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.StartSwagger();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
