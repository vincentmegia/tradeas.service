using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Tradeas.Colfinancial.Provider.Exceptions;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public class TaskProcessor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TaskProcessor));
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public TaskResult Process(List<Task> tasks, CancellationTokenSource cancellationTokenSource)
        {
            var isBackOfficeUpdating = false;
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(x =>
                {
                    if (x is BackOfficeOfflineException)
                    {
                        Logger.Error("Detected that backoffice is updating, cancelling all threads.");
                        cancellationTokenSource.Cancel();
                        isBackOfficeUpdating = true;
                        return true;
                    }
                    return true;
                });
            }

            return new TaskResult {IsSuccessful = isBackOfficeUpdating};
        }
    }
}