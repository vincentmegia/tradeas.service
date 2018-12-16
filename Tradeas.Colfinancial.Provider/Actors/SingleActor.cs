using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Actors
{
    public class SingleActor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SingleActor));

        private readonly TaskProcessor _taskProcessor;
        private readonly TaskActor _taskActor;
        
        public SingleActor(TaskActor taskActor)
        {
            _taskActor = taskActor;
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
                var taskResult =_taskActor.Do(batch);
                Logger.Info($"Thread id: {_taskActor.TaskInstance.Id} will processing the following symbols: { transactionParameter.Symbol }");
                isBackOfficeException = _taskProcessor
                    .Process(new List<Task>() { _taskActor.TaskInstance }, _taskActor.CancellationTokenSourceInstance)
                    .IsSuccessful.Value;

                if (taskResult.IsSuccessful.Value) break;
            }
            _taskActor.Dispose();
            
            return new TaskResult {IsSuccessful = true};
        }
    }
}