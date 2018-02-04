using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tradeas.Models;
using Tradeas.Repositories;
using Tradeas.Service.Api.Builders;

namespace Tradeas.Service.Api.Processors
{
    public class JournalStageProcessor : IJournalStageProcessor
    {
        private readonly IJournalStageRepository _journalStageRepository;
        private readonly IJournalRepository _journalRepository;
        private readonly ITransactionRepository _transactionRepository;

        public JournalStageProcessor(IJournalStageRepository journalStageRepository,
                                     IJournalRepository journalRepository,
                                     ITransactionRepository transactionRepository)
        {
            _journalStageRepository = journalStageRepository;
            _journalRepository = journalRepository;
            _transactionRepository = transactionRepository;
        }


        /// <summary>
        /// Process this instance.
        /// </summary>
        /// <returns>The process.</returns>
        public async Task<Result> Process()
        {
            //1) retrieve all open ideas
            var journalAwaiter = await _journalRepository.GetIdeasOpenStatus();

            //2) retrieve all transactitons having null for position id
            var transactionsAwaiter = await _transactionRepository.GetOrphanTransactions();

            var ideas =  journalAwaiter.GetData<List<Idea>>();
            var transactions = transactionsAwaiter.GetData<List<Transaction>>();
            //Task.WaitAll(new[] { journalAwaiter, transactionsAwaiter });
            //select all transactions for the open ideas and link it
            //var ideas = journalAwaiter.Result.GetData<List<Idea>>();
            //var transactions = transactionsAwaiter.Result.GetData<List<Transaction>>();
            transactions = transactions
                .OrderBy(transaction => transaction.CreatedDate)
                .ToList();

            var ideaBuilder = new IdeaBuilder()
                .OpenIdeas(ideas, transactions);

            //no ideas case
            //group remaining transactions by symbol whre posititionId == 0
            //create ideas
            //loop remaining transactions and keep tallying buy adn sell until mtached
            var newIdeas = transactions
                .Where(transaction => !string.IsNullOrEmpty(transaction.PositionId))
                .GroupBy(transaction => transaction.Symbol)
                .Select(transaction =>
                        new Idea
                        {
                            Id = $"{transaction.Key}{DateTime.Now.ToString("MMMMddyyyyhhmmss")}",
                            Symbol = transaction.Key,
                            Type = string.Empty,
                            Stars = new List<Star>(),
                            Status = "open",
                            Position = new Position
                            {
                                TransactionId = transaction.FirstOrDefault().TransactionId.ToString(),
                                OrderId = transaction.FirstOrDefault().OrderId.ToString(),
                                Symbol = transaction.Key,
                                Status = "executed",
                            },
                            EntryDate = DateTime.UtcNow
                        })
                .ToList();

            ideaBuilder.NewIdeas(newIdeas, transactions);
            var stageIdeas = new List<string>();

            stageIdeas.AddRange(ideas.Select(idea => JsonConvert.SerializeObject(idea)));
            stageIdeas.AddRange(newIdeas.Select(idea => JsonConvert.SerializeObject(idea)));

            var taskResult = await _journalStageRepository.BulkAsync(stageIdeas);
            return taskResult;
        }
    }
}
