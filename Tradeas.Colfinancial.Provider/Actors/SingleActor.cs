using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using log4net;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Actors
{
    public class SingleActor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SingleActor));

        private readonly TaskProcessor _taskProcessor;
        private readonly TaskActor _taskActor;
        private readonly IImportTrackerRepository _importTrackerRepository;
        
        public SingleActor(TaskActor taskActor,
            TaskProcessor taskProcessor,
            IImportTrackerRepository importTrackerRepository)
        {
            _taskActor = taskActor;
            _taskProcessor = taskProcessor;
            _importTrackerRepository = importTrackerRepository;
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

                
                var batch = new List<Import> { new Import { Symbol = transactionParameter.Symbol } };
                var taskResult =_taskActor.Do(batch, transactionParameter);
                Logger.Info($"Thread id: {_taskActor.TaskInstance.Id} will processing the following symbols: { transactionParameter.Symbol }");
                _taskProcessor.AddTask(_taskActor);
                isBackOfficeException = _taskProcessor
                    .Process()
                    .IsSuccessful.Value;

                if (taskResult.IsSuccessful.Value) break;
            }
            _taskActor.Dispose();
            
            var importTracker = _importTrackerRepository
                .GetAll()
                .GetData<List<ImportTracker>>()
                .ToList()
                .First(x => x.Equals(new ImportTracker(transactionParameter.Symbol)));

            importTracker.Status = "Success";
            _importTrackerRepository.PutAsync(importTracker);
            
            return new TaskResult {IsSuccessful = true};
        }
    }
}