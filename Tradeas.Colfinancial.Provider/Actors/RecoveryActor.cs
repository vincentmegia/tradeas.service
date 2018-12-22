using System;
using System.Collections.Generic;
using System.Threading;
using log4net;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Actors
{
    public class RecoveryActor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(RecoveryActor));
        private readonly ImportProcessor _importProcessor;
        private readonly BatchProcessor _batchProcessor;
        private readonly TaskProcessor _taskProcessor;
        private readonly List<TaskActor> _taskActors;
        
        public RecoveryActor(ImportProcessor importProcessor,
                             BatchProcessor batchProcessor,
                             TaskProcessor taskProcessor,
                             List<TaskActor> taskActors)
        {
            _importProcessor = importProcessor;
            _batchProcessor = batchProcessor;
            _taskProcessor = taskProcessor;
            _taskActors = taskActors;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Do(TransactionParameter transactionParameter)
        {
            var isBackOfficeException = false;
            var sleepInterval = 5;
            while (true)
            {
                if (isBackOfficeException)
                {
                    Logger.Info($"Performing sleep for {sleepInterval} minutes");
                    Thread.Sleep(TimeSpan.FromMinutes(sleepInterval));
                    sleepInterval *= 2;
                    Logger.Info("awoken from sleep");
                }

                _batchProcessor.SkipCounter = 0;
                foreach (var taskActor in _taskActors)
                {
                    var batch = _batchProcessor
                            .Process(ImportMode.Retry)
                            .GetData<List<Import>>();
                    if (batch.Count == 0) continue;
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                    taskActor.Do(batch, transactionParameter);
                }

                isBackOfficeException = _taskProcessor
                    .Process()
                    .IsSuccessful
                    .Value;

                _taskProcessor.Dispose();
                
                var isCompleted = _importProcessor.IsCompleted();
                if (isCompleted)
                    break;
            }
            
            return new TaskResult {IsSuccessful = true};
        }
    }
}