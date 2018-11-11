using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;
using Newtonsoft.Json;
using NUnit.Framework;
using Tradeas.Models;

namespace Tradeas.Repositories.Tests
{
    [TestFixture]
    public class TransactionRepositoryTest
    {
        [Test]
        public async Task GetAllTest()
        {
            var client = new MyCouchClient("https://admin:calv1nc3@tradeasdb.southeastasia.cloudapp.azure.com:6984", "tradeas");
            //var client = new MyCouchClient("http://127.0.0.1:5984", "brokers");
            //var temp = new MyCouchClient("http://127.0.0.1:5984", "brokers-temp");
            while (true)
            {
                var queryViewRequest = new QueryViewRequest("query", "new-view").Configure(query =>
                    query.IncludeDocs(true)
                    .Limit(1000));
                var response = new ViewQueryResponse<Models.BrokerTransaction>();
                try
                {
                    response = await client.Views.QueryAsync<Models.BrokerTransaction>(queryViewRequest);
                    var transactions = response
                        .Rows
                        .Select(row => row.Value)
                        .ToList();

                    if (response.Rows.Length <= 0) break;
                    
                    var list = new List<string>();
                    foreach (var transaction in transactions)
                    {
//                        transaction.Code = transaction.Id;
//                        transaction.Id += "-broker";
//                        transaction.Rev = null;
                        //var json = JsonConvert.SerializeObject(new BrokerJson(transaction));
//                        list.Add(json);
                        //temp.Documents.PutAsync(null, null)
                        var a = client.Documents.DeleteAsync(transaction.Id, transaction.Rev);
                    }

                    var t = await client.Documents.BulkAsync(new BulkRequest().Include(list.ToArray()));
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
