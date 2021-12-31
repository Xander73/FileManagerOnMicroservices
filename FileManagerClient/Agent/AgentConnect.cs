using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileManagerClient.Agent.Client.Interface;
using FileManagerClient.Agent.Models.Responses;
using FileManagerClient.Agent.Models.Requests;

namespace FileManagerClient.Agent
{
    public class AgentConnect
    {
        private string BaseAddresInformator { get; set; } = "http://localhost:44396";
        private string BaseAddresChanger { get; set; } = "http://localhost:";


        public AgentConnect(string[] args)
        {

        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            }).UseNLog();


        public static AllDrivesResponse GetAllDrivers (IFileManagerInformatorClient fileManagerInformatorClient)
        {
            var requestDrivers = new AllDrivesRequest()
            {
                ClientBaseAddres = @"https://localhost:44396"
            };

            return fileManagerInformatorClient.GetDrives(requestDrivers);
        }
    }
}
