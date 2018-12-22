using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Tradeas.Colfinancial.Provider.Actors;
using Tradeas.Colfinancial.Provider.Exceptions;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public class TaskProcessor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TaskProcessor));
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly List<TaskActor> _taskActors;

        public TaskProcessor(CancellationTokenSource cancellationTokenSource,
            List<TaskActor> actors)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _taskActors = actors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskActor"></param>
        public void AddTask(TaskActor taskActor)
        {
            _taskActors.Add(taskActor);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Process()
        {
            var isBackOfficeUpdating = false;
            try
            {
                var tasks = _taskActors
                        .Where(taskActor => taskActor.TaskInstance != null)
                        .Select(taskActor => taskActor.TaskInstance);
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(x =>
                {
                    if (x is BackOfficeOfflineException)
                    {
                        Logger.Error("Detected that backoffice is updating, cancelling all threads.");
                        _cancellationTokenSource.Cancel();
                        isBackOfficeUpdating = true;
                        return true;
                    }
                    return true;
                });
            }

            return new TaskResult {IsSuccessful = isBackOfficeUpdating};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Dispose()
        {
            foreach (var taskActor in _taskActors)
            {
                taskActor.Dispose();
            }

            return new TaskResult {IsSuccessful = true};
        }
    }
}