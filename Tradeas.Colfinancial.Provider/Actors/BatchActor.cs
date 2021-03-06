﻿using System;
using System.Collections.Generic;
using System.Threading;
using log4net;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Actors
{
    public class BatchActor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BatchActor));
        private readonly ImportProcessor _importProcessor;
        private readonly BatchProcessor _batchProcessor;
        private readonly TaskProcessor _taskProcessor;
        private readonly List<TaskActor> _taskActors;
        
        public BatchActor(ImportProcessor importProcessor,
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
            _importProcessor.PurgeTrackers(transactionParameter);
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
                            .Process(ImportMode.Batch)
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
                if (isCompleted) break;
                //send signal to recovery actor
            }
            
            return new TaskResult {IsSuccessful = true};
        }
    }
}