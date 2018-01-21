using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Tradeas.Colfinancial.Provider;
using Tradeas.Models;

namespace Tradeas.Service.Api.Controllers
{
    [Route("api/colfinancial")]
    public class ColfinancialController : Controller
    {
        private readonly IExtractor _extractor;
        private static ILog Logger = LogManager.GetLogger(typeof(ColfinancialController));

        public ColfinancialController(IExtractor extractor)
        {
            _extractor = extractor;
        }

        /// <summary>
        /// Extracts the transactions, GET colfinancial/transactions
        /// </summary>
        /// <returns>The transactions.</returns>
        /// <param name="transactionParameter">Transaction parameter.</param>
        [HttpPost]
        [Route("transactions/extract")]
        public async Task<string> ExtractTransactions([FromBody]TransactionParameter transactionParameter)
        {
            try
            {
                await _extractor.Extract(transactionParameter);
            }
            catch(Exception e) 
            {
                Logger.Error(e);
                return e.Message;
            }
            return $"transaction {transactionParameter.Frequency} extraction complete";
        }

        [HttpPost]
        [Route("ideas/stage")]
        public async Task<string> Stage()
        {
            try
            {
                await _extractor.Extract(transactionParameter);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return e.Message;
            }
            return $"ideas has been staged.";
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
