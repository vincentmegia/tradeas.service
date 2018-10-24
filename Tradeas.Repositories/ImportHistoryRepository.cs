using System;
using log4net;
using MyCouch;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class ImportHistoryRepository : Repository, IImportHistoryRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ImportHistoryRepository));

        public ImportHistoryRepository(string serverAddress) : base(serverAddress, "imports-history")
        {}

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public TaskResult GetByDate(DateTime date)
        {
            Logger.Info($"retrieving import history for date: {date:yyyy-MM-dd}");
            var response = this.Entities.GetAsync<ImportHistory>(date.ToString("yyyy-MM-dd"));
            var result = new TaskResult { IsSuccessful = true };
            result.SetData(response.Result.Content);
            Logger.Info($"import history retrieved: {response.Result.Content}");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="importHistory"></param>
        /// <returns></returns>
        public TaskResult Add(ImportHistory importHistory)
        {
            Logger.Info($"adding new import history: {importHistory}");
            var response = this.Entities.PutAsync(importHistory.Id, importHistory);
            var result = new TaskResult { IsSuccessful = true };
            Logger.Info($"add completed. reason {response.Result.Reason}");
            return result;
        }
   }
}