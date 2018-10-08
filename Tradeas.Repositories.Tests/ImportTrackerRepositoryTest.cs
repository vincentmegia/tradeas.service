using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Contexts;
using MyCouch.Requests;
using NUnit.Framework;
using Tradeas.Models;

namespace Tradeas.Repositories.Tests
{
    [TestFixture]
    public class ImportTrackerRepositoryTest
    {
        [Test]
        public void BuildImports()
        {
            var importTrackerRepository = new ImportTrackerRepository("http://127.0.0.1:5984");
            var exportedList = new List<string>
            {
                "2GO", "ACE","FGENF","FGENG","FJP","FJPB","FLI","FMETF","FOOD","FPH","FPI","GEO","GERI","GLO","GMA7","GMAP","GPH","GSMI","GTCAP","H2O",
                "HI","HLCM","HOUSE","I","IMP","ION","IPO","IRC","IMP","ION","IPO","IRC","IS","ISM","JFC","JGS","JOH","KEP","KPH","KPHB","LAND","LC","LCB",
                "LFM","LMG","LOTO","LPZ","LR","LRP","LRW","LSC","LTG","MA","MAB","MAC","MACAY","MAH","MARC","MB","MBC","MBT","MED","MEG","MER","MFC","MFIN",
                "MG","MHC","MJC","MJIC","MRC","MVC","MWC","MWIDE",",NI","NIKL","NOW","NRCP","OM","OPM","ORE","OV","PA","PAL","PBB","PBC","PCOR","PERC",
                "PGOLD","PHA","PHES","PHN","PIP","PMPC","PNB","PNX","PPC","PRC","PRIM","PRMX","PSB","PSE","PTC","PX","PXP","RCB","RCI","REG","RFM","RLT",
                "ROCK","ROX","RRHI","RWM","SCC","SECB","SEVN","SFI","SFIP","SGI","SHNG","SLF","SLI","SM","SMC","SMC2B","GOB","IMPB","AEV",
                "SMPH","SOC","SPC","SPM","STI","STR","SUN","T","TBGI","TEL","TFC","TFHI","UBP","UNI","UPM","URC","V","VITA","VLL","VMC","VUL","VVT",
                "WEB","WIN"
            };

            foreach (var exported in exportedList)
            {
                var item = new ImportTracker(exported);
                var response = importTrackerRepository.PostAsync(item);
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void A()
        {
            var client = new MyCouchClient("https://tradeasdb.southeastasia.cloudapp.azure.com:6984",
                "broker-transactions");
            var queryViewRequest = new QueryViewRequest("query", "new-view")
                .Configure(c => c.IncludeDocs(true));
            var response =  client.Views.QueryAsync<BrokerTransaction>(queryViewRequest);
            var rows = response.ConfigureAwait(true).GetAwaiter().GetResult().Rows;


            foreach (var resultRow in rows)
            {
                var item = resultRow.Value;
                item.CreatedDate = new DateTime(2018, 10, 01);
                try
                {
                    var result = client.Entities.PutAsync(item.Id, item.Rev, item);
                    Console.WriteLine(item);
                    Trace.WriteLine(item);
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