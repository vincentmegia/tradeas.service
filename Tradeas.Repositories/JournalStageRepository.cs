using System.Collections.Generic;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Requests;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class JournalStageRepository : MyCouchClient, IJournalStageRepository
    {
        public JournalStageRepository(string serverAddress) : base(serverAddress, "journals-stage")
        {}


        /// <summary>
        /// Add the specified ideas.
        /// </summary>
        /// <returns>The add.</returns>
        /// <param name="ideas">Ideas.</param>
        public async Task<Result> BulkAsync(List<string> ideasJson)
        {
            var request = new BulkRequest();
            request.Include(ideasJson.ToArray());
            var response = await Documents.BulkAsync(request);
            return new TaskResult
            {
                IsSuccessful = response.IsSuccess,
                StatusCode = response.StatusCode.ToString(),
                Reason = response.Reason
            };
        }
    }
}