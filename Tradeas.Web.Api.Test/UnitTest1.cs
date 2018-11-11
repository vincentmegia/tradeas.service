using System;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;

namespace Tradeas.Web.Api.Test
{
    [TestFixture]
    public class UnitTest1
    { 
        [Test]
        public void Test1()
        {
            var _restClient = new RestClient("https://tradeasdb.southeastasia.cloudapp.azure.com:6984/_session");
            _restClient.Authenticator = new HttpBasicAuthenticator("admin", "calv1nc3");
            var request = new RestRequest() {Method = Method.POST, RequestFormat = DataFormat.Json};
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            var json = new { name = "dmt", password = "test"};
            request.AddJsonBody(json);
            
            var response = _restClient.Execute(request);
        }
    }
}