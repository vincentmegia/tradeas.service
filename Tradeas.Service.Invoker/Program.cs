using System;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Tradeas.Service.Invoker
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        
        static void Main(string[] args)
        {
            InitializeLogger();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            var endpointUrl = configuration["Endpoint:Url"];
            Logger.Info($"endpoint url: {endpointUrl}");

            var resource = configuration["Endpoint:Resource"];
            Logger.Info($"endpoint resource: {resource}");
            
            var client = new RestClient(endpointUrl);
            var request = new RestRequest(resource);
            request.AddHeader("Content-Type", "application/json");
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new TransactionParameter
            {
                Frequency = configuration["TransactionParameter:Frequency"],
                LoginCredential = new TransactionParameter.Credential
                {
                    Username = configuration["TransactionParameter:LoginCredential:Username"], 
                    Password = configuration["TransactionParameter:LoginCredential:Password"]
                }
            });
            Logger.Info("executing tradeas.service broker extract http post");
            var response = client.Execute(request);
            Logger.Info($"response: {response.Content}");
        }

        /// <summary>
        /// 
        /// </summary>
        private static void InitializeLogger()
        {
            var log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config.xml"));
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(Hierarchy));
            XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}