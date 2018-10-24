using System;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Tradeas.Models;
using Tradeas.Web.Api.Services;

namespace Tradeas.Web.Api.Controllers
{
    [Route("api/chart")]
    public class ChartControllerController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ChartControllerController));
        private readonly IAuthenticationService _authenticationService;

        public ChartControllerController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        
        // GET
        [AllowAnonymous]
        [HttpPost("history")]
        public string GetHistory(string symbol, string resolution, string from, string to)
        {
            var client = new RestClient("https://webapi.investagrams.com");
            var request = new RestRequest("InvestaApi/TradingViewChart/history");
            request
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Origin", "https://www.investagrams.com")
                .AddQueryParameter("symbol", symbol)
                .AddQueryParameter("resolution", resolution)
                .AddQueryParameter("from", from)
                .AddQueryParameter("to", to);
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);
            Logger.Info($"response: {response.Content}");

            return response.Content;
        }
        
        // GET
        [AllowAnonymous]
        [HttpPost("symbols")]
        public string GetSymbol(string symbol)
        {
            var client = new RestClient("https://webapi.investagrams.com");
            var request = new RestRequest("InvestaApi/TradingViewChart/history");
            request
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Origin", "https://www.investagrams.com")
                .AddQueryParameter("symbol", symbol);

            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);
            Logger.Info($"response: {response.Content}");

            return response.Content;
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }
    }
}