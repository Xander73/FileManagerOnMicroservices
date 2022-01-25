using FileManagerClient.Agent.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System;

namespace FileManagerClient.Agent
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection servises)
        {
            servises.AddHttpClient<FileManagerInformatorAgentClient, FileManagerInformatorAgentClient>()
                .AddTransientHttpErrorPolicy(p => p
                .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
                
            servises.AddControllers();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
                                

        }
    }
}
