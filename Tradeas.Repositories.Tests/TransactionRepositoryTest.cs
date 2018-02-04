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
            var client = new MyCouchClient("http://127.0.0.1:5984", "transactions");
            var queryViewRequest = new QueryViewRequest("query", "all").Configure(query => query.IncludeDocs(true));
            var response = new ViewQueryResponse<Transaction>();
            try
            {
                response = await client.Views.QueryAsync<Transaction>(queryViewRequest);
                var transactions = response
                .Rows
                .Select(row => (Transaction)JsonConvert.DeserializeObject(row.IncludedDoc, typeof(Transaction))) // iad to resort to this, freakin framework works finicky
                .ToList();

                var list = new List<string>();
                foreach (var transaction in transactions)
                {
                    if (transaction.PositionId == null)
                        transaction.PositionId = "0";
                    var json = JsonConvert.SerializeObject(transaction);
                    list.Add(json);
                }

                var t = await client.Documents.BulkAsync(new BulkRequest().Include(list.ToArray()));
            }
            catch (Exception e)
            {

            }
        }
    }
}
