using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using Tradeas.Models;

namespace Tradeas.Repositories.Tests
{
    [TestFixture]
    public class ImportRepositoryTest
    {
        [Test]
        public async Task BuildImports()
        {
            var securityRepository = new SecurityRepository("http://127.0.0.1:5984");
            var securityResult =  await securityRepository.GetAll();
            
            var securities = securityResult.GetData<List<Security>>();
            var importRepository = new ImportRepository("http://127.0.0.1:5984");
            foreach (var security in securities)
            {
                var import = new Import
                {
                    Symbol = security.Symbol,
                    Type = "broker-transactions",
                    CreatedBy = "iamstupendous"
                };
                var json = JsonConvert.SerializeObject(import, new IsoDateTimeConverter { DateTimeFormat = Models.Constants.DateFormat });
                var postResponse = await importRepository.Documents.PostAsync(json);
            }
            Assert.IsTrue(true);
        }
    }
}