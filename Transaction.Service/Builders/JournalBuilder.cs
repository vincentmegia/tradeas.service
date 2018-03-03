using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Service.Api.Builders
{
    public class JournalBuilder : IJournalBuilder
    {
        private readonly TransactionBuilder _transactionBuilder;
        private readonly JournalRepository _journalRepository;
        protected List<Idea> Ideas { get; set; }
        protected List<Transaction> Transactions { get; set; }

        public JournalBuilder(TransactionBuilder transactionBuilder,
                              JournalRepository journalRepository)
        {
            _transactionBuilder = transactionBuilder;
            _journalRepository = journalRepository;
        }

        public List<Position> Positions => throw new NotImplementedException();

        /// <summary>
        /// Build the specified rows.
        /// </summary>
        /// <returns>The build.</returns>
        /// <param name="rows">Rows.</param>
        public JournalBuilder Build(ReadOnlyCollection<IWebElement> rows)
        {
            //get open ideas
            var result = _journalRepository
                .GetIdeasOpenStatus()
                .GetAwaiter()
                .GetResult();
            Ideas = result.GetData<List<Idea>>();

            //get all transactions
            Transactions = _transactionBuilder.Build(rows);
            return this;
        }

        /// <summary>
        /// Suggests the ideas.
        /// </summary>
        /// <returns>The ideas.</returns>
        public JournalBuilder CreateStageIdeas()
        {
            foreach (var idea in Ideas)
            {
                var orphanTransactions = Transactions.FindAll(t => t.Symbol.ToLower() == idea.Symbol.ToLower() &&
                                                              string.IsNullOrEmpty(t.PositionId));
                foreach (var orphanTransaction in orphanTransactions)
                {
                    orphanTransaction.PositionId = idea.Id;
                    idea.Position.TransactionIds.Add(orphanTransaction.Id);
                    idea.Position.Transactions.Add(orphanTransaction);
                }
            }
            return this;
        }

        /// <summary>
        /// Ops the implicit.
        /// </summary>
        /// <returns>The implicit.</returns>
        /// <param name="journalBuilder">Journal builder.</param>
        public static implicit operator List<Idea>(JournalBuilder journalBuilder)
        {
            return journalBuilder.Ideas;
        }
    }
}
