using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Microsoft.Extensions.Configuration;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public class BatchProcessor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionProcessor));
        private readonly IConfiguration _configuration;
        private readonly ImportProcessor _importProcessor;
        private int _skipCounter;
        

        public BatchProcessor(IConfiguration configuration,
                              ImportProcessor importProcessor)
        {
            _configuration = configuration;
            _importProcessor = importProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Process()
        {
            var imports = _importProcessor
                .Process()
                .GetData<List<Import>>();
            Logger.Info($"imports count {imports.Count}");

            var workerCount = Convert.ToInt32(_configuration["WorkerCount"]);
            Logger.Info($"setting worker count {workerCount}");
            
            var batchSize = imports.Count / workerCount;
            Logger.Info($"batch size per worker {batchSize}");
            if (batchSize == 0) batchSize = imports.Count;
            var skipCounter = 0;
            var batch = imports
                .Skip(skipCounter)
                .Take(batchSize)
                .ToList();
            _skipCounter += batchSize;

            var taskResult = new TaskResult {IsSuccessful = true};
            taskResult.SetData(batch);
            return taskResult;
        }
    }
}