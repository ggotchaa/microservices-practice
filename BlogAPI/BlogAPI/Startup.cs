using BlogAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogAPI", Version = "v1" });
            });

            // Add your DbContext configuration
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Add RabbitMQ communication configuration
            var factory = new ConnectionFactory
            {
                HostName = "localhost", // Replace with RabbitMQ server hostname if different
                UserName = "guest",     // Replace with RabbitMQ username if different
                Password = "guest"      // Replace with RabbitMQ password if different
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            services.AddSingleton<IConnection>(connection);
            services.AddSingleton<IModel>(channel);
            services.AddSingleton<IBlogCommunicationService, BlogCommunicationService>();

            services.AddScoped<IBlogCommunicationService, BlogCommunicationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogAPI v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Start consuming RabbitMQ messages
            var blogCommunicationService = app.ApplicationServices.GetRequiredService<IBlogCommunicationService>();
            blogCommunicationService.StartConsuming();
        }
    }
}
