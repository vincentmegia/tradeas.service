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
        public int SkipCounter { get; set; }
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
        public TaskResult Process(ImportMode importMode)
        {
            //if (_imports == null) 
                _imports = _importProcessor
                .Process(importMode)
                .GetData<List<Import>>();
            Logger.Info($"imports count {_imports.Count}");

            var workerCount = Convert.ToInt32(_configuration["WorkerCount"]);
            Logger.Info($"setting worker count {workerCount}");

            var batchSize = 0;
            if (_imports.Count > workerCount && (_imports.Count <= workerCount * 2))
            {
                batchSize = _imports.Count;               
            }
            else
            {
                batchSize = _imports.Count / workerCount;
            }

            Logger.Info($"batch size per worker {batchSize}");
            if (batchSize == 0) batchSize = _imports.Count;
            var batch = _imports
                .Skip(SkipCounter)
                .Take(batchSize)
                .ToList();
            SkipCounter += batchSize;

            var taskResult = new TaskResult {IsSuccessful = true};
            taskResult.SetData(batch);
            return taskResult;
        }
    }
}