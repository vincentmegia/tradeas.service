using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Tradeas.Colfinancial.Provider;
using Tradeas.Models;
using Tradeas.Web.Api.Processors;

namespace Tradeas.Web.Api.Controllers
{
    [Route("api/colfinancial")]
    public class ColfinancialController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ColfinancialController));
        private readonly IExtractor _extractor;
        private readonly IJournalStageProcessor _journalStageProcessor;

        public ColfinancialController(IExtractor extractor,
                                      IJournalStageProcessor journalStageProcessor)
        {
            _extractor = extractor;
            _journalStageProcessor = journalStageProcessor;
        }

        [HttpGet]
        [Route("check")]
        public string Check()
        {
            return "Success";
        }
        
        /// <summary>
        /// Extracts the transactions, GET colfinancial/transactions
        /// </summary>
        /// <returns>The transactions.</returns>
        /// <param name="transactionParameter">Transaction parameter.</param>
        [HttpPost]
        [Route("transactions/extract")]
        public string ExtractTransactions([FromBody]TransactionParameter transactionParameter)
        {
            try
            {
                _extractor.Extract(transactionParameter);
            }
            catch(Exception e) 
            {
                Logger.Error(e);
                return e.Message;
            }
            return $"transaction {transactionParameter.Frequency} extraction complete";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("journal-stage/stage")]
        public async Task<string> Stage()
        {
            try
            {
                await _journalStageProcessor.Process();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return e.Message;
            }
            return $"ideas has been staged.";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("transactions/fix")]
        public async Task<string> Fix()
        {
            try
            {
                await _journalStageProcessor.Process();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return e.Message;
            }
            return $"ideas has been staged.";
        }
    }
}
