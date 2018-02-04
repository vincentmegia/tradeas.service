using System;
using System.Collections.Generic;
using System.Linq;
using Tradeas.Models;

namespace Tradeas.Service.Api.Builders
{
    public class IdeaBuilder
    {
        public IdeaBuilder Build() => this;


        /// <summary>
        /// The result.
        /// </summary>
        public IdeaBuilder OpenIdeas(List<Idea> ideas,
                                     List<Transaction> transactions)
        {
            //open ideas case
            //get all transctions and sort by dates
            //loop and keep tallying until buy and sell are matched
            //marked transaction.positionId as none 0
            foreach (var idea in ideas)
            {
                var nodelessTransactions = transactions
                    .Where(transaction => transaction.Symbol.Equals(idea.Symbol, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
                if (!idea.Position.IsOpen()) continue;
                if (nodelessTransactions
                    .FirstOrDefault()
                    .Side
                    .Equals("sell", StringComparison.CurrentCultureIgnoreCase)) continue;

                foreach (var nodelessTransaction in nodelessTransactions)
                {
                    idea.Position.TransactionIds.Add(nodelessTransaction.Id);
                    idea.Position.Transactions.Add(nodelessTransaction);
                    nodelessTransaction.PositionId = idea.Position.Id; //mark transaction already linked
                    if (!idea.Position.IsOpen()) break;
                }
            }

            return this;
        }

        /// <summary>
        /// News the ideas.
        /// </summary>
        /// <returns>The ideas.</returns>
        /// <param name="ideas">Ideas.</param>
        /// <param name="transactions">Transactions.</param>
        public IdeaBuilder NewIdeas(List<Idea> ideas,
                                    List<Transaction> transactions)
        {
            //open ideas case
            //get all transctions and sort by dates
            //loop and keep tallying until buy and sell are matched
            //marked transaction.positionId as none 0
            foreach (var idea in ideas)
            {
                var nodelessTransactions = transactions
                    .Where(transaction => transaction.Symbol.Equals(idea.Symbol, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();

                idea.Position.CreatedDate = nodelessTransactions.FirstOrDefault().CreatedDate;
                foreach (var nodelessTransaction in nodelessTransactions)
                {
                    idea.Position.TransactionIds.Add(nodelessTransaction.Id);
                    idea.Position.Transactions.Add(nodelessTransaction);

                    nodelessTransaction.PositionId = idea.Position.Id; //mark transaction already linked
                    if (!idea.Position.IsOpen()) break;
                }
            }

            return this;
        }
    }
}
