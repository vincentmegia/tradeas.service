using System;
using System.Diagnostics;
using System.Globalization;
using MyCouch;
using MyCouch.Requests;
using NUnit.Framework;
using Tradeas.Models;

namespace Tradeas.Repositories.Tests
{
    [TestFixture]
    public class BrokerTransactionTest
    {
        [Test]
        public void Test()
        {
            var client = new MyCouchClient("https://admin:calv1nc3@tradeasdb.southeastasia.cloudapp.azure.com:6984",
                "broker-transactions");
            var queryViewRequest = new QueryViewRequest("query", "new-view")
                .Configure(c => c.IncludeDocs(true).Limit(1000));
            var response =  client.Views.QueryAsync<BrokerTransaction>(queryViewRequest);
            var rows = response.ConfigureAwait(true).GetAwaiter().GetResult().Rows;



            foreach (var resultRow in rows)
            {
                var datePart = resultRow.Value.Id.Split('-')[1];
                var date = DateTime.ParseExact(datePart, "yyyyMMMdd", CultureInfo.InvariantCulture);
                var brokerTransaction = resultRow.Value;
                if (date.CompareTo(brokerTransaction.CreatedDate) == 0) continue;
                
                try
                {
                    Console.WriteLine("adjusting create date for, broker transaction: " + brokerTransaction);
                    brokerTransaction.CreatedDate = date;
                    var result = client.Entities.PutAsync(brokerTransaction.Id, brokerTransaction.Rev, brokerTransaction);
                    Console.WriteLine("operation status: " + result.Result.Reason);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);                    
                }
            }
            Assert.IsTrue(true);
        }
    }
}