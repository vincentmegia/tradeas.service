using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using log4net;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Models;

namespace Tradeas.Colfinancial.Provider.Builders
{
    public class TransactionBuilder : IBuilder
    {
        private readonly static ILog Logger = LogManager.GetLogger(typeof(TransactionBuilder));
        public List<Transaction> Transactions { get; }

        public TransactionBuilder()
        {
            Transactions = new List<Transaction>();
        }

        /// <summary>
        /// Trades the scrapper. builders. IB uilder. build.
        /// </summary>
        /// <param name="rows">Rows.</param>
        public TransactionBuilder Build(ReadOnlyCollection<IWebElement> rows)
        {
            foreach (var row in rows)
            {
                var columns = row.FindElements(By.TagName("td"));
                var transactionId = columns[1].Text;
                if (transactionId.ToLower() == "trx#") continue;

                var symbol = columns[4].Text;
                var quantity = columns[5].Text;
                var matchedQuantity = columns[6].Text;
                var price = columns[7].Text;
                var side = columns[8].Text;
                var status = columns[9].Text;

                var transaction = new Transaction
                {
                    _id = transactionId,
                    Symbol = symbol,
                    Quantity = Convert.ToDecimal(quantity),
                    MatchedQuantity = Convert.ToDecimal(matchedQuantity),
                    Price = Convert.ToDecimal(price),
                    Side = side,
                    Status = status
                };
                Transactions.Add(transaction);
                Logger.Info(transaction);
            }
            return this;
        }

        /// <summary>
        /// Ops the implicit.
        /// </summary>
        /// <returns>The implicit.</returns>
        /// <param name="transactionBuilder">Transaction builder.</param>
        public static implicit operator List<Transaction>(TransactionBuilder transactionBuilder)
        {
            return transactionBuilder.Transactions;
        }
    }
}
