using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Tradeas.Models;

namespace Tradeas.Web.Api.Services
{
    public class BrokerExtractService : BackgroundService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private readonly IConfiguration _configuration;
        private Timer _timer;

        public BrokerExtractService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public override async Task StopAsync (CancellationToken stoppingToken)
        {
            _timer.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void DoWork(object state)
        {
            var scheduledTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 45, 0);
            if (DateTime.Now.TimeOfDay <= scheduledTime.TimeOfDay) return;
                
            var endpointUrl = _configuration["Endpoint:Url"];
            Logger.Info($"endpoint url: {endpointUrl}");

            var resource = _configuration["Endpoint:Resource"];
            Logger.Info($"endpoint resource: {resource}");
            
            var client = new RestClient(endpointUrl);
            var request = new RestRequest(resource);
            request.AddHeader("Content-Type", "application/json");
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new TransactionParameter
            {
                Frequency = _configuration["TransactionParameter:Frequency"],
                LoginCredential = new Credential                {
                    Username = _configuration["TransactionParameter:LoginCredential:Username"], 
                    Password = _configuration["TransactionParameter:LoginCredential:Password"]
                }
            });
            Logger.Info("executing tradeas.service broker extract http post");
            var response = client.Execute(request);
            Logger.Info($"response: {response.Content}");
        }
    }
}