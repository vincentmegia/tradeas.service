using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class JournalRepository : MyCouchClient, IJournalRepository
    {
        public JournalRepository(string serverAddress) : base(serverAddress, "journal")
        {}

        /// <summary>
        /// Gets the idea.
        /// </summary>
        /// <returns>The idea.</returns>
        /// <param name="idea">Idea.</param>
        public async Task<Result<Idea>> GetIdea(Idea idea)
        {
            return new Result<Idea>();
        }

        /// <summary>
        /// Gets the idea.
        /// </summary>
        /// <returns>The idea.</returns>
        /// <param name="transaction">Transaciton.</param>
        public async Task<Result<Idea>> GetIdea(Transaction transaction)
        {
            var document = await Documents.GetAsync(transaction.Id);
            var result  = new Result<Idea> { Instance = new Idea{}, IsSuccessful = true };
            result.Messages.Add(document.Error);
            result.Messages.Add(document.Reason);
            return result;
        }

        /// <summary>
        /// Gets the idea open status.
        /// </summary>
        /// <returns>The idea open status.</returns>
        public async Task<Result<List<Idea>>> GetIdeasOpenStatus()
        {
            var queryViewRequest = new QueryViewRequest("idea", "status").Configure(c => c.Key("open").IncludeDocs(true));
            var response = await Views.QueryAsync<Idea>(queryViewRequest)
                as ViewQueryResponse<Idea>;
            
            var ideas = response.Rows.Select(row => row.Value);
            var result = new Result<List<Idea>> { Instance = ideas.ToList(), IsSuccessful = true };
            result.Messages.Add(response.Error);
            result.Messages.Add(response.Reason);
            return result;
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