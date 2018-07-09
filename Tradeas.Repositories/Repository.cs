using System.Collections.Generic;
using log4net;
using MyCouch;
using MyCouch.Requests;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public abstract class Repository : MyCouchClient, IRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransactionRepository));
        
        protected Repository (string serverAddress, string database) : base(serverAddress, database)
        {}
        
        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="items">Ideas json.</param>
        public TaskResult BulkAsync(List<string> items)
        {
            Logger.Info($"performing bulk imports for broker transaction total: {items.Count}");
            var request = new BulkRequest();
            request.Include(items.ToArray());
            var response = Documents.BulkAsync(request);
            Logger.Info($"operaton reason: {response.Result.StatusCode}");
            return new TaskResult();
        }
    }
}