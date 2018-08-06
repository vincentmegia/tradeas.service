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
        private List<Import> _imports;

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
            if (_imports ==null) _imports = _importProcessor
                .Process()
                .GetData<List<Import>>();
            Logger.Info($"imports count {_imports.Count}");

            var workerCount = Convert.ToInt32(_configuration["WorkerCount"]);
            Logger.Info($"setting worker count {workerCount}");
            
            var batchSize = _imports.Count / workerCount;
            Logger.Info($"batch size per worker {batchSize}");
            if (batchSize == 0) batchSize = _imports.Count;
            var batch = _imports
                .Skip(_skipCounter)
                .Take(batchSize)
                .ToList();
            _skipCounter += batchSize;

            var taskResult = new TaskResult {IsSuccessful = true};
            taskResult.SetData(batch);
            return taskResult;
        }
    }
}