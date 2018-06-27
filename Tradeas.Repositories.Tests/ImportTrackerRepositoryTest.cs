using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using Tradeas.Models;

namespace Tradeas.Repositories.Tests
{
    [TestFixture]
    public class ImportTrackerRepositoryTest
    {
        [Test]
        public async Task BuildImports()
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
                var response = await importTrackerRepository.PostAsync(item);
            }
            Assert.IsTrue(true);
        }
    }
}