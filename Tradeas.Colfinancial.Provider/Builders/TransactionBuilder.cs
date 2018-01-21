using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using log4net;
using OpenQA.Selenium;
using Tradeas.Models;
using Tradeas.Strategies;

namespace Tradeas.Colfinancial.Provider.Builders
{
    public class TransactionBuilder : ITransactionBuilder
    {
        private readonly static ILog Logger = LogManager.GetLogger(typeof(TransactionBuilder));
        private const string DateTimeFormat = "MM/dd/yyyy HH:mm:ss";
        public List<Transaction> Transactions { get; }
        //private readonly IdStrategy _idStrategy;

        public TransactionBuilder()
        {
            Transactions = new List<Transaction>();
            //_idStrategy = idStrategy;
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

                var orderId = columns[2].Text;
                var createdDate = columns[3].Text;
                var symbol = columns[4].Text;
                var quantity = columns[5].Text;
                var matchedQuantity = columns[6].Text;
                var price = columns[7].Text;
                var side = columns[8].Text;
                var status = columns[9].Text;

                var id = $"col{transactionId}{orderId}{symbol}{matchedQuantity.Replace(",", string.Empty).Replace(".", string.Empty)}{price.Replace(",", string.Empty).Replace(".", string.Empty)}";
                var transaction = new Transaction
                {
                    Id = id,
                    TransactionId = long.Parse(transactionId, NumberStyles.Any),
                    OrderId = long.Parse(orderId, NumberStyles.Any),
                    Symbol = symbol,
                    Quantity = Convert.ToDecimal(quantity),
                    MatchedQuantity = Convert.ToDecimal(matchedQuantity),
                    Price = Convert.ToDecimal(price),
                    Side = (side.ToLower() == "bn") ? "Buy" : "Sell",
                    Status = status,
                    CreatedDate = Convert.ToDateTime(createdDate)
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
