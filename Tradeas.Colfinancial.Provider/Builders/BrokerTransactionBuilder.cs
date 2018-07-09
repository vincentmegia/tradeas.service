using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Builders
{
    public class BrokerTransactionBuilder
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionBuilder));
        public List<BrokerTransaction> Transactions { get; }

        public BrokerTransactionBuilder()
        {
            Transactions = new List<BrokerTransaction>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public BrokerTransactionBuilder Build(List<IWebElement> rows, string symbol)
        {
            var brokerTransaction = new BrokerTransaction
            {
                Id = $"{symbol}-{DateTime.Now:yyyyMMMdd}",
                Symbol = symbol
            };

            foreach (var row in rows)
            {
                var columns = row.FindElements(By.TagName("td"));
                try
                {
                    var code = columns[1].Text;
                    var buyVolume = columns[3].Text;
                    var buyAmount = columns[4].Text;
                    var buyAverage = columns[5].Text;
                    var sellVolume = columns[6].Text;
                    var sellAmount = columns[7].Text;
                    var sellAverage = columns[8].Text;
                    var netAmount = columns[9].Text;
                    var totalValue = columns[10].Text;

                    var brokerTransactionDetail = new BrokerTransactionDetail
                    {
                        Code = code,
                        BuyVolume = decimal.Parse(buyVolume),
                        BuyAmount = decimal.Parse(buyAmount),
                        BuyAverage = decimal.Parse(buyAverage),
                        SellVolume = decimal.Parse(sellVolume),
                        SellAmount = decimal.Parse(sellAmount),
                        SellAverage = decimal.Parse(sellAverage),
                        NetAmount = decimal.Parse(netAmount),
                        TotalValue = decimal.Parse(totalValue)
                    };
                    brokerTransaction.Details.Add(brokerTransactionDetail);
                }
                catch (WebDriverException e)
                {
                    Logger.Error("error has been encountered trying to retrieve broker infos from columns.",e);
                    throw;
                }
            }
            Transactions.Add(brokerTransaction);
            Logger.Info($"created brokerTransaction: {brokerTransaction}");
            return this;
        }

        /// <summary>
        /// Ops the implicit.
        /// </summary>
        /// <returns>The implicit.</returns>
        /// <param name="brokerTransactionBuilder">Transaction builder.</param>
        public static implicit operator List<BrokerTransaction>(BrokerTransactionBuilder brokerTransactionBuilder)
        {
            return brokerTransactionBuilder.Transactions;
        }
    }
}