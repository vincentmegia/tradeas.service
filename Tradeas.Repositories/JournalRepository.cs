using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Responses;
using Newtonsoft.Json;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class JournalRepository : MyCouchClient, IJournalRepository
    {
        public JournalRepository(string serverAddress) : base(serverAddress, "journals")
        {
        }

        /// <summary>
        /// Gets the idea.
        /// </summary>
        /// <returns>The idea.</returns>
        /// <param name="idea">Idea.</param>
        public async Task<Result> GetIdea(Idea idea)
        {
            return new TaskResult();
        }

        /// <summary>
        /// Gets the idea.
        /// </summary>
        /// <returns>The idea.</returns>
        /// <param name="transaction">Transaciton.</param>
        public async Task<Result> GetIdea(Transaction transaction)
        {
            var document = await Documents.GetAsync(transaction.Id);
            var result  = new TaskResult { IsSuccessful = true };
            result.Messages.Add(document.Error);
            result.Messages.Add(document.Reason);
            return result;
        }

        /// <summary>
        /// Gets the idea open status.
        /// </summary>
        /// <returns>The idea open status.</returns>
        public async Task<Result> GetIdeasOpenStatus()
        {
            var queryViewRequest = new QueryViewRequest("ideas", "status").Configure(c => c.Key("open").IncludeDocs(true));
            var response = await Views.QueryAsync<Idea>(queryViewRequest)
                as ViewQueryResponse<Idea>;
            
            var ideas = response.Rows.Select(row => (Idea)JsonConvert.DeserializeObject(row.IncludedDoc, typeof(Idea)));
            var taskResult = new TaskResult
            {
                IsSuccessful = true,
                Messages = new List<string>
                {
                    response.Error,
                    response.Reason
                }
            };
            taskResult.SetData(ideas.ToList());
            return taskResult;
        }

        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="ideasJson">Ideas json.</param>
        public async Task BulkAsync(List<string> ideasJson)
        {
            var request = new BulkRequest();
            request.Include(ideasJson.ToArray());
            var response = await Documents.BulkAsync(request);
        }
    }
}