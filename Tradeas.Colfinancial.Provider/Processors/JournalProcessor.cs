using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public class JournalProcessor : IJournalProcessor
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IJournalStageRepository _journalStageRepository;
        private readonly IJournalRepository _journalRepository;
        private readonly static ILog Logger = LogManager.GetLogger(typeof(JournalProcessor));
        private const string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public JournalProcessor(ITransactionRepository transactionRepository,
                                IJournalStageRepository journalStageRepository,
                                IJournalRepository journalRepository)
        {
            _transactionRepository = transactionRepository;
            _journalStageRepository = journalStageRepository;
            _journalRepository = journalRepository;
        }

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <param name="ideas">Transactions.</param>
        public async Task UpdateIdeas(List<Idea> ideas)
        {
            try
            {
                var ideasJson = new List<string>();
                var transactionsJson = new List<string>();

                foreach (var idea in ideas)
                {
                    var ideaJson = JsonConvert.SerializeObject(idea, new IsoDateTimeConverter { DateTimeFormat = DateFormat });
                    ideasJson.Add(ideaJson);

                    foreach (var transaction in idea.Position.Transactions)
                    {
                        var transactionJson = JsonConvert.SerializeObject(transaction, new IsoDateTimeConverter { DateTimeFormat = DateFormat });
                        transactionsJson.Add(transactionJson);
                    }
                }

                //await _journalStageRepository.BulkAsync(ideasJson);
                await _journalRepository.BulkAsync(ideasJson);
                await _transactionRepository.BulkAsync(transactionsJson);
            }
            catch(Exception e)
            {
                //need some code to handle bubbling of error in task
                Logger.Error(e);
                throw;
            }
        }
    }
}
